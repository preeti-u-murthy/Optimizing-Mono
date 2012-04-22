before="$(date +%s%N)";
gmcs $1 -r:Mono.Cecil.dll;
mono $2;
after="$(date +%s%N)";
elapsedsec="$(expr $after - $before)";
echo $before;
echo $after;
echo $elapsedsec;
