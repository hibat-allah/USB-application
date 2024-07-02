First thing, you should download postgres and install it in the default port 543. <br />
Choose a name for your DB and set a password to the default user in the DB. In this case the DB name is "entreprisebd".<br />
After installation, <br />
connect to the DB from a cmd window using this command:
```c
psql -U postgres

```

and then tap your password.
after that, write the folowing line to connect to the DB schema you created while installing:

```c
\c entreprisebd
```

when you cant to create another user write:

```c
CREATE USER zflub WITH PASSWORD 'M#N@d31N';
```

and if you try to connect to the DB with that user:

```c
psql -d entreprisebd -U zflub
```
