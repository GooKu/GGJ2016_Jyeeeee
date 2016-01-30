using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CSVReader
{
	static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
	static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
	static char[] TRIM_CHARS = { '\"' };
//gooku/-s	
	public static List<Dictionary<string, string>> Read(string file)
	{
		var list = new List<Dictionary<string, string>>();
/*
	public static List<Dictionary<string, object>> Read(string file)
	{
			var list = new List<Dictionary<string, object>>();
*/
//gooku/-e
		TextAsset data = Resources.Load (file) as TextAsset;
		
		var lines = Regex.Split (data.text, LINE_SPLIT_RE);
		
		if(lines.Length <= 1) return list;
		
		var header = Regex.Split(lines[0], SPLIT_RE);
		for(var i=1; i < lines.Length; i++) {
			
			var values = Regex.Split(lines[i], SPLIT_RE);
			if(values.Length == 0 ||values[0] == "") continue;
//gooku/-s			
			var entry = new Dictionary<string, string>();
//			var entry = new Dictionary<string, object>();
//gooku/-e
			for(var j=0; j < header.Length && j < values.Length; j++ ) {
				string value = values[j];
				value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
//gooku/-s
				entry[header[j]] = value;

/*
				object finalvalue = value;
				int n;
				float f;
				if(int.TryParse(value, out n)) {
					finalvalue = n;
				} else if (float.TryParse(value, out f)) {
					finalvalue = f;
				}
				entry[header[j]] = finalvalue;
*/
//gooku/-e
			}
			list.Add (entry);
		}
		return list;
	}

	public static List<Dictionary<string, string>> ReadFile(TextAsset data)
	{
		return ReadFile (data.text);
/*
		var list = new List<Dictionary<string, string>>();

		var lines = Regex.Split (data.text, LINE_SPLIT_RE);
		
		if(lines.Length <= 1) return list;
		
		var header = Regex.Split(lines[0], SPLIT_RE);
		for(var i=1; i < lines.Length; i++) {
			
			var values = Regex.Split(lines[i], SPLIT_RE);
			if(values.Length == 0 ||values[0] == "") continue;
			//gooku/-s			
			var entry = new Dictionary<string, string>();
			//			var entry = new Dictionary<string, object>();
			//gooku/-e
			for(var j=0; j < header.Length && j < values.Length; j++ ) {
				string value = values[j];
				value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
				entry[header[j]] = value;
			}
			list.Add (entry);
		}
		return list;
*/		
	}

	public static List<Dictionary<string, string>> ReadFile(String _text)
	{
		var list = new List<Dictionary<string, string>>();
		
		var lines = Regex.Split (_text, LINE_SPLIT_RE);

		if(lines.Length <= 1) return list;
		
		var header = Regex.Split(lines[0], SPLIT_RE);

		for(var i=1; i < lines.Length; i++) {
			var values = Regex.Split(lines[i], SPLIT_RE);

			if(values.Length == 0 ||values[0] == "") continue;
			//gooku/-s			
			var entry = new Dictionary<string, string>();
			//			var entry = new Dictionary<string, object>();
			//gooku/-e
			for(var j=0; j < header.Length && j < values.Length; j++ ) {
				string value = values[j];
				value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
				entry[header[j]] = value;
			}
			list.Add (entry);
		}
		return list;
	}

	public static string GetDataById(List<Dictionary<string, string>> _list, string _id, string _key){//gooku: this api suitable one time seach
		for(int i =0; i < _list.Count;i++){
			if(_id != _list[i]["id"])
				continue;
			return _list[i][_key];
		}
		return "not found data";
	}
}