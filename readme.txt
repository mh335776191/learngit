排除超过100M的文件
git filter-branch --force --index-filter "git rm --cached --ignore-unmatch Project1/Project1.1\ Sample\ Project/output.txt"  --prune-empty --tag-name-filter cat -- --all
git commit --amend -CHEAD
git push origin master

sql server 9003 
先在数据新建一个同样名的数据库，然后停止服务器的服务，删除新建的日志文件，然后用原mdf文件去替换掉新建的mdf文件，再启动服务器，会出现该数据库置疑。
数据库当出现置疑时，可以通过以下语句来解决：
Use Master
Go
sp_configure 'allow updates', 1
reconfigure with override
Go
alter database dbname set emergency //MSSQL2005进入紧急模式的方法
go
alter database dbname set single_user //进入单用户模式
go
dbcc checkdb('dbname',REPAIR_ALLOW_DATA_LOSS) //.重建日志文件
go
alter database dbname set multi_user //5．恢复多用户模式
go
alter database dbname set online //6．恢复非紧急模式
go