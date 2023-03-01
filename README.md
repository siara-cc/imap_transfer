# IMAP Transfer 

Transfer folders and messages from one imap mailbox to another.

Sometimes there is a need for transferring to a new mail server for various reasons.  This program helps in transferring all the messages to maintain continuity with older conversations.

# Usage

Clone this repo, modify Program.cs and provide following details:

- Old Server IMAP Address
- Old Server email address
- Old Server password
- New Server IMAP Address
- New Server email address
- New Server password
- Folders that need to be transferred

Mail servers don't accept user entered passwords anymore and require user to generate app passwords from their websites.  In this case, the passwords above are to be filled using corresponding App passwoords generated.

Then install .Net SDK if it is not already available and run `dotnet run` from command terminal with this repo as the current folder.

Sometimes the server times out after transferring many messages.  The program is expected to retry, but in case it stops, following 2 variable can be modified to indicate the folder and message from which to resume:

```c#
int lastFolder = 0;
int lastIdProcessed = 0;
```

# Credit

I thank the author of the excellent libraries `MailKit` and `MimeKit` [Jeffrey Stedfast](https://github.com/jstedfast) on which this repo is built.
