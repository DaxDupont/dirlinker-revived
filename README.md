# dirlinker-revived
Directory Linker is a small Winforms application that allows the user to quickly and easily create symbolic links in XP, Vista and Windows 7 for files and folders.

# Project Description
Directory Linker is a small Winforms application that allows the user to quickly and easily create symbolic links in XP, Vista and Windows 7 for files and folders. 

The process for creating a symbolic directory link for a folder that already exists is a bit clunky, you have to:
Create the target
Copy any existing content to the target
Delete the folder you want to be a link
Start cmd.exe and then enter the mklink command along with the full paths of the folders you want to link

DirLinker helps with this, it is very basic UI that allows you to enter where you want the link and where you want to link to. You can just delete the link place or copy the contents over first. Use these options at your own risk, I will not be held responsible for any data loss.

# Features
Create symbolic folder links
Create symbolic file links (Only on Vista or later)
Move or delete a file/folder before creating a link, making it easier to create a link for a file/folder that already exists
Undo support, if anything goes wrong we can put it back.

# Requirements:
XP or later
NET 3.5
Administrative rights
