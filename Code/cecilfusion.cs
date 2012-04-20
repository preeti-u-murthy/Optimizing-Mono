//Loop fusion of simple execution time for loops
//program containing only two loops can be fused

//INPUT: program to be optimized
//OUTPUT: optimized CIL written to another assembly

using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Reflection;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

using Mono.Collections.Generic;

public class cecilfusion{
public static void Main(string[] args)
{
int i=0,k=0;

AssemblyDefinition myLibrary = AssemblyDefinition.ReadAssembly (args[0]);


//Gets all types which are declared in the Main Module of "MyLibrary.dll"
//Here types refers to classes
foreach (TypeDefinition type in myLibrary.MainModule.Types) {

	//Get all methods in the class
	foreach( MethodDefinition method in type.Methods){

		if(method.FullName=="System.Void Program1::Method1(System.Int32[],System.Int32[])")
		{


		i=0;
		k=0;
		int[] j=new int[5];	//j is used to hold the index of the method instruction before add
		int[] branch=new int[5]; //to store the indices of branch instructions in eachloop of the method
		int[] ldloc=new int[5]; //to store the indices of the instruction till which the body needs to be replicated
		int[] cond=new int[5];

		Instruction condTargetInstr=method.Body.Instructions[0];
		

		//Gets the CilWorker of the method for working with CIL instructions
		ILProcessor worker = method.Body.GetILProcessor();


		foreach( Instruction instruct in method.Body.Instructions)
		{
				

				//take the instruction number of branch
				if(instruct.OpCode.FlowControl==FlowControl.Branch) {
					branch[k]=i;

				}
				
				if(instruct.OpCode.FlowControl==FlowControl.Cond_Branch) {
				

				
					condTargetInstr=method.Body.Instructions[i];

					j[k]=i-5;
					ldloc[k]=i-6;
					cond[k]=i-1;
					k++;
				}
				

				i++;


		}

		
		

		FieldInfo[] myFields = method.Body.Instructions[j[0]].GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance
            | BindingFlags.Public);




		if(myFields[1].GetValue(method.Body.Instructions[j[0]]).Equals(myFields[1].GetValue(method.Body.Instructions[j[1]]))
		&&(myFields[2].GetValue(method.Body.Instructions[cond[0]]).Equals(myFields[2].GetValue(method.Body.Instructions[cond[1]])))
		) //checking if the increment variable and loop bound are same for two loops to be fused
		{
			
			//insert the body of the 2nd loop into the body of the first loop
			int beginBody=branch[1]+1;
			int endBody=ldloc[1]-1;
			
			numberOfInstructionsToReplicate=(endBody-beginBody)+1;
			int numberOfInstructionsToEliminate=numberOfInstructionsToReplicate+1+1+6;
			//1 for branch instr,1 for ldloc instr,6 for the
			//remaining loop testing instructions	
			


			//replicating the body of loop2 within loop1
			//for(i=1;i<k;i++)
			i=1;

			
				for(int l=1;l<=numberOfInstructionsToReplicate;l++)
				{
					Instruction ins = method.Body.Instructions[branch[i]+l];
				
					worker.InsertBefore(method.Body.Instructions[ldloc[0]], ins);	
					ldloc[0]++;	
					branch[i]++;
					ldloc[1]++;
					
					
				}
					
					
					//remove the second loop 
				
				{
				
					worker.Remove(method.Body.Instructions[branch[1]]);
					worker.Remove(method.Body.Instructions[ldloc[1]-1]);
					worker.Remove(method.Body.Instructions[ldloc[1]-1]);		
					worker.Remove(method.Body.Instructions[ldloc[1]-1]);	
					worker.Remove(method.Body.Instructions[ldloc[1]-1]);	
					worker.Remove(method.Body.Instructions[ldloc[1]-1]);	
					worker.Remove(method.Body.Instructions[ldloc[1]-1]);	
					worker.Remove(method.Body.Instructions[ldloc[1]-1]);
					
						
				}
				
				
		}
	}


		myLibrary.Write(args[1]);
		
	}
}


}


}






