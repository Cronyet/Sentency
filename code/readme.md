# Sentency Project
## Sentency (Socket Version)
### Server Program
Welcome to the Sentency
You're now reading the Sentency readme.md file.

### Struct
The server was built with C#, and using no Visual Studio but Visual Studio Code
in order to keep the code runs every platform without much edit.

### Performance
Accroding to our test, the server program use less than 5 MB memory in Windows11
when serving 9 clients.

### Compile
In the `code` directory, you can run:
1. WindowsOS with C# Compile Tools (.NET SDK)
``` CMD
csc Sentency.cs server.cs serverlib.cs MD5.cs /out:server.exe
```
2. Linux (Mostly Ubuntu)
Coming soon.

### Test
You can compile the connect.cs to test the server locally.
``` CMD
csc connect.cs /out:test.exe
```
Then:
1. Run the server.exe
2. Type 'y' and press Enter
3. Use `help` to read the help doc
4. Init the content xml doc path with `init` command like:
``` Sentency
init D:\Sentency\src\content\content.xml
```
5. Load all sentencies into program with `load` command.
6. Run test.exe and input 127.0.0.1 to connect to the local server.
7. Receive sentencies.

### Bugs
1. With the bug of foreaching Dictionary in System.Collections.Genric
sometimes the bug appear but sometimes disappear.
We are trying to repair it.

## Sentency (API Version)
Developing...
