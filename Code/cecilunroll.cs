//Loop unrolling of simple execution time for loops
//programs containing array assignments are only unrolled 

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



public class cecilunroll{
public static void Main(string[] args)
{

int i=0,k=0;


AssemblyDefinition myLibrary = AssemblyDefinition.ReadAssembly (args[0]);


//Gets all types which are declared in the Main Module of "MyLibrary.dll"
//Here types refers to classes
foreach (TypeDefinition type in myLibrary.MainModule.Types) {

	//Get all methods in the class
	foreach( MethodDefinition method in type.Methods){
		
		if(method.FullName=="System.Void Program1::Method1(System.Int32[])")
		{
		i=0;
		k=0;
		int[] j=new int[5];	//j is used to hold the index of the method instruction before add in the loop testing part of the cil
		int[] branch=new int[5]; //to store the indices of branch instructions in eachloop of the method
		int[] ldloc=new int[5]; //to store the indices of the instruction till which the body needs to be replicated
		

		Instruction condTargetInstr=method.Body.Instructions[0];


		//Gets the CilWorker of the method for working with CIL instructions
		ILProcessor worker = method.Body.GetILProcessor();


		/***************************EXPERIMENTING WITH ADDING A LOCAL VARIABLE TO THE ASSEMBLY***********************************/


		foreach( Instruction instruct in method.Body.Instructions)
		{
				

				//take the instruction number of branch
				if(instruct.OpCode.FlowControl==FlowControl.Branch) {
					branch[k]=i;
					
				}
				
				if(instruct.OpCode.FlowControl==FlowControl.Cond_Branch) {
					
					condTargetInstr=method.Body.Instructions[i];
					j[k]=i-7;
					ldloc[k]=i-8;
					k++;
				}

				i++;
				if(instruct.OpCode.Code==Code.Stelem_I4)				
				//Console.WriteLine(instruct.OpCode.Code.ToString());
				{
					usearray=true;
				}
		}

			//only if the loop contains an array are we loop unrolling
			if(usearray==true)
			{
				//changing the increment variables of all loops
				for(i=0;i<k;i++){
					Instruction ins = method.Body.Instructions[j[i]];

					//Change the increment variable to 2
					Instruction changeIncrVarSentence;
					changeIncrVarSentence = worker.Create(OpCodes.Ldc_I4_2);

					worker.Replace(ins,changeIncrVarSentence);
					

				}

				//for each loop
				for(i=0;i<k;i++)
				{

	
					 if(i==0&&method.FullName=="System.Void Program1::Method1(System.Int32[])"){
					 
					 	//creating the loop body which will be inserted "array[i+1]=i+1"
					 	
						Instruction ins1,ins2,ins3,ins4,ins5,ins6,ins7,ins8;
				
						ins1= worker.Create(OpCodes.Ldarg_0);	//load the array

						ins2= worker.Create(OpCodes.Ldloc_0);	//load the current array index
						ins3= worker.Create(OpCodes.Ldc_I4_1);	//load 1 onto stack
						ins4= worker.Create(OpCodes.Add);		//add 1+arrayindex

						ins5= worker.Create(OpCodes.Ldloc_0);	//load value onto stack
						ins6= worker.Create(OpCodes.Ldc_I4_1);	//load 1 onto stack
						ins7= worker.Create(OpCodes.Add);		// i+1	

						ins8= worker.Create(OpCodes.Stelem_I4);	// array[arrayindex+1]=i+1;

					
					
						//inserting the above instructions after the loop body "array[i]=i"
						//before the loop testing condition
						
						worker.InsertBefore(method.Body.Instructions[ldloc[i]], ins1);	
						ldloc[i]++;
						worker.InsertBefore(method.Body.Instructions[ldloc[i]], ins2);	
						ldloc[i]++;	
						worker.InsertBefore(method.Body.Instructions[ldloc[i]], ins3);	
						ldloc[i]++;	
						worker.InsertBefore(method.Body.Instructions[ldloc[i]], ins4);	
						ldloc[i]++;	
						worker.InsertBefore(method.Body.Instructions[ldloc[i]], ins5);	
						ldloc[i]++;
						worker.InsertBefore(method.Body.Instructions[ldloc[i]], ins6);	
						ldloc[i]++;		
						worker.InsertBefore(method.Body.Instructions[ldloc[i]], ins7);	
						ldloc[i]++;		
						worker.InsertBefore(method.Body.Instructions[ldloc[i]], ins8);	
						ldloc[i]++;				
				
					  }	
					  if(i==1) {
						Instruction ins9,ins10,ins11,ins12;
				

						branch[i]=branch[i]+8;
				
					  }
				}//end of for loop
			}// end of if loop	
		}
	}//end of for each method
}//end of for each type
		myLibrary.Write(args[1]);



}

}






