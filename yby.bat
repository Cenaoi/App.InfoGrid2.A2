C:\Windows\Microsoft.NET\Framework\v2.0.50727\aspnet_compiler -v / -p .\View -f -errorstack -fixednames .\‘§±‡“Î > log_‘§±‡“Î.txt
aspnet_merge.exe .\‘§±‡“Î -prefix ascx.InfoGrid2

xcopy .\‘§±‡“Î\bin\App.*.dll .\bin\ /-Y /Y
xcopy .\‘§±‡“Î\bin\ascx.*.View.dll .\bin\ /-Y /Y
xcopy .\‘§±‡“Î\bin\ascx.*.*.View.dll .\bin\ /-Y /Y