@echo off
FOR /L %%i IN (1,1,10) DO (
copy input%%i.txt input.txt
start Antiplagiat.exe
echo --------------------input%%i.txt-------------------------
type output.txt
echo ====Real solution====:
type res\0%%i.a
)
type res\10.a
pause