C:\Windows\Microsoft.NET\Framework\v2.0.50727\aspnet_compiler -v / -p .\View -f -errorstack -fixednames .\Ԥ���� > log_Ԥ����.txt
aspnet_merge.exe .\Ԥ���� -prefix ascx.InfoGrid2

xcopy .\Ԥ����\bin\App.*.dll .\bin\ /-Y /Y
xcopy .\Ԥ����\bin\ascx.*.View.dll .\bin\ /-Y /Y
xcopy .\Ԥ����\bin\ascx.*.*.View.dll .\bin\ /-Y /Y