using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;

namespace OCInject
{
    public class OCInjectException : Exception
    {
        public OCInjectException(String message)
            : base(message)
        { }
    }

    public class ClassFactory
    {
        public class TypeOptions<TRegisteredClass> where TRegisteredClass : class
        {
            private ClassFactory _currentFactory;

            public TypeOptions(ClassFactory classFactory)
            {
                _currentFactory = classFactory;    
            }

            public TypeOptions<TRegisteredClass> WithFactory<T>()
            {
                _currentFactory.RegisterDelegateFactoryForType<TRegisteredClass, T>();
                return this;
            }

            public TypeOptions<TRegisteredClass> AsSingleton()
            {
                _currentFactory.AddTypeToSingletonList(typeof(TRegisteredClass));
                return this;
            }


            public TypeOptions<TRegisteredClass> AlwaysReturnObject(TRegisteredClass obj)
            {
                _currentFactory._SingletonList.Add(typeof(TRegisteredClass), obj);
                return this;
            }
        }

        private readonly IDictionary<Type, Type> _types = new Dictionary<Type, Type>();
        private readonly IDictionary<Type, Delegate> _typeFactories = new Dictionary<Type, Delegate>();
        private readonly IDictionary<Type, Object> _SingletonList = new Dictionary<Type, Object>();

        public Func<Type, Object> CallToResolve { get; set; } 
        
        public void AddFactory(Type contract, Delegate factory)
        {
            _typeFactories.Add(contract, factory);    
        }

        public virtual TypeOptions<TContract> RegisterType<TContract, TImplementation>()
            where TImplementation : class, TContract
            where TContract : class
        {
            _types[typeof(TContract)] = typeof(TImplementation);

            return new TypeOptions<TContract>(this);
        }

        public virtual T ManufactureType<T>()
        {
            return ManufactureType<T>(new Object[] {});
        }

        public virtual T ManufactureType<T>(params Object[] args)
        {
            return (T)Resolve(typeof(T), args);
        }

        public ClassFactory CreateChildContainer()
        {
            return new ClassFactory { CallToResolve = typeToResolve => Resolve(typeToResolve) };
        }

        public virtual Object Resolve(Type contract, params Object[] args)
        {
          
            if (IsSingleton(contract, args))
            {
                return _SingletonList[contract];
            }

            return InternalResolve(contract, args);
        }

        public TypeOptions<T> RegisterType<T>()
            where T : class
        {
            Type registeringType = typeof(T);
            _types.Add(registeringType, registeringType);

            return new TypeOptions<T>(this);
        }

        private Boolean IsSingleton(Type contract, Object[] args)
        {
            Object singletonObject;
            if (_SingletonList.TryGetValue(contract, out singletonObject))
            {
                if (singletonObject == null)
                {
                    singletonObject = InternalResolve(contract, args);
                    _SingletonList[contract] = singletonObject;
                }
                return true;
            }
            return false;
        }

        private Object InternalResolve(Type contract, params Object[] args)
        {

            if (!_types.ContainsKey(contract))
            {
                Object instance = CallExternalToResolve(contract);
                if (instance == null)
                {
                    throw new OCInjectException("contract is not a known type");
                }
                return instance;
            }
         
            Type implementation = _types[contract];

            ConstructorInfo constructor = SelectBestConstructor(implementation.GetConstructors(), args);

            ParameterInfo[] constructorParameters = constructor.GetParameters();

            if (constructorParameters.Length == 0)
                return Activator.CreateInstance(implementation);

            List<Object> parameters = new List<Object>(constructorParameters.Length);
            List<Object> arguments = new List<Object>(args);

            foreach (ParameterInfo parameterInfo in constructorParameters)
            {
                parameters.Add(ResolveConstructorArgs(arguments, parameterInfo));
            }

            return constructor.Invoke(parameters.ToArray());
        
        }

        private ConstructorInfo SelectBestConstructor(ConstructorInfo[] avaliableConstructors, Object[] resolveTimeArgs)
        {
            List<Object> potentialArgsList = new List<Object>(resolveTimeArgs);
     
            var sortedbyLargest = avaliableConstructors.OrderByDescending(ci => ci.GetParameters().Length);
            foreach (var ci in sortedbyLargest)
            {
                if (ci.GetParameters().ToList().TrueForAll(pi => CanResolve(pi.ParameterType, potentialArgsList)))
                    return ci;
            }
      
            throw new OCInjectException("No resolvable constructor found");
        }

        private Object CallExternalToResolve(Type contract)
        {
            Func<Type, Object> callee = CallToResolve;
            if (callee != null)
            {
                return callee(contract);
            }
            return null;
        }

        private bool CanResolve(Type type)
        {
            return CanResolve(type, new List<Object>());
        }

        private Boolean CanResolve(Type type, List<Object> resolveTimeArgs)
        {
            Object matchingParam = resolveTimeArgs.FirstOrDefault(o => o.GetType() == type);
            if (matchingParam != null)
            {
                resolveTimeArgs.Remove(matchingParam);
                return true;
            }
          
            else
            {
                return _types.ContainsKey(type) ||  IsFactory(type);
            }
        }

        private bool IsFactory(Type type)
        {
            if (_typeFactories.ContainsKey(type))
            {
                return true;
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Func<>))
            {
                return CanResolve(type.GetGenericArguments()[0]); // we only support func<t>
            }

            return false;

        }

        private Object ResolveConstructorArgs(IList<Object> args, ParameterInfo parameterInfo)
        {
            Object param = args.FirstOrDefault(o => parameterInfo.ParameterType.IsAssignableFrom(o.GetType()));

            if (param != null)
            {
                args.Remove(param);
            }
            else if (parameterInfo.ParameterType.IsGenericType && parameterInfo.ParameterType.GetGenericTypeDefinition() == typeof(Func<>))
            {
                param = GetFuncFactory(parameterInfo.ParameterType);
            }
            else if (_typeFactories.ContainsKey(parameterInfo.ParameterType))
            {
                param = _typeFactories[parameterInfo.ParameterType];
            }
            else
            {
                param = Resolve(parameterInfo.ParameterType);
            }

            return param;
        }

        private Object GetFuncFactory(Type funcType)
        {
            MethodInfo mi = typeof(ClassFactory).GetMethod("FactoryTemplate");
            mi = mi.MakeGenericMethod(funcType.GetGenericArguments());

            Expression call = Expression.Call(mi, new Expression[] {Expression.Constant(this), 
                Expression.NewArrayInit(typeof(Object), new Expression[] {}) } );

            return Expression.Lambda(call, new ParameterExpression[] { }).Compile();            
        }

        public virtual void RegisterDelegateFactoryForType<TResult, TFactoryDelegateType>()
        {
            MethodInfo delegateInvoker = typeof(TFactoryDelegateType).GetMethod("Invoke");
            ParameterExpression[] factoryParams = GetParamsAsExpressions(delegateInvoker);

            //Build the factory from the template that looks roughly like
            // (param1, paramn) => FactoryTemplate(classFactory, param1, paramn)
            MethodInfo mi = typeof(ClassFactory).GetMethod("FactoryTemplate");
            mi = mi.MakeGenericMethod(typeof(TResult));

            Expression call = Expression.Call(mi, new Expression[] {Expression.Constant(this), 
                Expression.NewArrayInit(typeof(Object), factoryParams)} );

            TFactoryDelegateType factory = Expression.Lambda<TFactoryDelegateType>(call, factoryParams).Compile();

            _typeFactories.Add(typeof(TFactoryDelegateType), factory as Delegate);
        }

        private ParameterExpression[] GetParamsAsExpressions(MethodInfo mi)
        {
            List<ParameterExpression> paramsAsExpression = new List<ParameterExpression>();

            Array.ForEach<ParameterInfo>(mi.GetParameters(),
                p => paramsAsExpression.Add(Expression.Parameter(p.ParameterType, p.Name)));

            return paramsAsExpression.ToArray();
        }

        public static T FactoryTemplate<T>(ClassFactory factory, params Object[] args)
        {
            return factory.ManufactureType<T>(args);
        }

        public void Dispose()
        {
            foreach (var singleton in _SingletonList.Values)
            {
                IDisposable disposable = singleton as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            _typeFactories.Clear();
            _types.Clear();
        }

        protected void AddTypeToSingletonList(Type type)
        {
            _SingletonList.Add(type, null);
        }
    }
}