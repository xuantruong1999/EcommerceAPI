-----Docker Compose with https setting
- Refference:https://docs.microsoft.com/en-us/aspnet/core/security/docker-compose-https?view=aspnetcore-6.0

dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p "123456"
dotnet dev-certs https --trust

-----How to restore db (ecodb) to sql server on linux container? 

docker exec -it db mkdir /var/opt/mssql/backup

docker cp backup_db/ecodb.bak db:/var/opt/mssql/backup


docker exec -it db /opt/mssql-tools/bin/sqlcmd -S localhost `
   -U SA -P "sa_pass123456" `
   -Q "RESTORE FILELISTONLY FROM DISK = '/var/opt/mssql/backup/ecodb.bak'"
   
   
docker exec -it db /opt/mssql-tools/bin/sqlcmd `
   -S localhost -U SA -P "sa_pass123456" `
   -Q "RESTORE DATABASE EcommerceDB FROM DISK = '/var/opt/mssql/backup/ecodb.bak'"
   
docker exec -it db /opt/mssql-tools/bin/sqlcmd `
   -S localhost -U SA -P "sa_pass123456" `
   -Q "RESTORE DATABASE EcommerceDB FROM DISK = '/var/opt/mssql/backup/ecodb.bak' WITH MOVE 'EcommerceDB' TO '/var/opt/mssql/data/EcommerceDB.mdf', MOVE 'EcommerceDB_log' TO '/var/opt/mssql/data/EcommerceDB_log.ldf'"
   
   
------------
   to check 
   
   
   docker exec -it db /opt/mssql-tools/bin/sqlcmd `
   -S localhost -U SA -P "sa_pass123456" `
   -Q "SELECT * FROM Product"
   