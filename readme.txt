�ų�����100M���ļ�
git filter-branch --force --index-filter "git rm --cached --ignore-unmatch Project1/Project1.1\ Sample\ Project/output.txt"  --prune-empty --tag-name-filter cat -- --all
git commit --amend -CHEAD
git push origin master

sql server 9003 
���������½�һ��ͬ���������ݿ⣬Ȼ��ֹͣ�������ķ���ɾ���½�����־�ļ���Ȼ����ԭmdf�ļ�ȥ�滻���½���mdf�ļ���������������������ָ����ݿ����ɡ�
���ݿ⵱��������ʱ������ͨ����������������
Use Master
Go
sp_configure 'allow updates', 1
reconfigure with override
Go
alter database dbname set emergency //MSSQL2005�������ģʽ�ķ���
go
alter database dbname set single_user //���뵥�û�ģʽ
go
dbcc checkdb('dbname',REPAIR_ALLOW_DATA_LOSS) //.�ؽ���־�ļ�
go
alter database dbname set multi_user //5���ָ����û�ģʽ
go
alter database dbname set online //6���ָ��ǽ���ģʽ
go