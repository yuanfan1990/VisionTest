using System;
using System.Windows.Forms; 

using System.Collections.Generic;
using System.IO;



public class Script

{ 
	
	public Dictionary<string, object> InputRun() 
	
	{ 
	     
		Dictionary<string, object> dic = new Dictionary<string, object>();
            
		dic["Input1"] = 100;
            
		dic["Input2"] = "hello world";
            
		dic["Input4"] = 3.14;

            
		return dic;
	
	}
	
    
	public List<string> OutputRun(Dictionary<string, object> dic) 
	
	{ 
            
		//string s1 = dic["Output1"].ToString();

            
		string f1 = "AddStaticLabel(ok, 300, 300, red, 20)";
            
		string f2 = "SendResult(10, 5)";

            
		List<string> funs = new List<string>();
            
		funs.Add(f1);
            
		funs.Add(f2);
           
            
		return funs;
       
	}

}
