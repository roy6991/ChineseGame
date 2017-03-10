using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CharacterSet
{
    public int setID;
    public Dictionary<string, string> char_audioSet;
	private string charactor;
	private string audioUrl;
    public CharacterSet()
    {
        char_audioSet = new Dictionary<string, string>();
    }
	public string getCharactor(){
		return charactor;
	}
	public string getAudioUrl(){
		return audioUrl;
	}
	public void setCharactor(string charactor){
		this.charactor = charactor;
	}
	public void setAudioUrl(string audioUrl){
		this.audioUrl = audioUrl;
	}
}
