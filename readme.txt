�ų�����100M���ļ�
git filter-branch --force --index-filter "git rm --cached --ignore-unmatch Project1/Project1.1\ Sample\ Project/output.txt"  --prune-empty --tag-name-filter cat -- --all
git commit --amend -CHEAD
git push origin master