First thing, you should download postgres and install it in the default port 543.
choose a name for your DB and set a password to the default user in the DB. In this case the DB name is "entreprisebd".
after installation,
connect to the DB from a cmd window using this command:

$psql -U postgres$
-
and then tap your password.
after that, write the folowing line to connect to the DB schema you created while installing:

$\c entreprisebd$
-
when you cant to create another user write:

$CREATE USER zflub WITH PASSWORD 'M#N@d31N';$
-
and if you try to connect to the DB with that user:

$psql -d entreprisebd -U zflub$
-
