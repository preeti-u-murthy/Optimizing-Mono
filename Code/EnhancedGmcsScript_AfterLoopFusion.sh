#we havent used embedded mono in the script
before="$(date +%s%N)";
gmcs $1 -r:Mono.Cecil.dll;
#fusion.exe is the executable from the previous line
#try2.exe is the executable into which the modified cil will be written
mono cecilfusiontry.exe fusion.exe try2.exe; 
mono try2.exe;
after="$(date +%s%N)";
elapsedsec="$(expr $after - $before)";
echo $before;
echo $after;
echo $elapsedsec;
