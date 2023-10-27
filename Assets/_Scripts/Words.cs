using System;
using System.Linq;
using UnityEngine;

public class Words : MonoBehaviour
{
    [SerializeField] TextAsset WORD_LIST_RAW;
    private WordList words;

    // Start is called before the first frame update
    void Awake()
    {
        words = new WordList(WORD_LIST_RAW);
    }
    
    public bool isWord(string word)
    {
        return words.Contains(word);
    }

    class WordList
    {
        public readonly string[] WordArray;
        public WordList(TextAsset wl)
        {
            if (wl == null)
            {
                Debug.LogError("NO WORD LIST FOUND...");
                return;
            }
            WordArray = wl.text.Split("\n");
        }

        public bool Contains(string word)
        {
            word = word.ToUpper();
            return WordArray.Contains(word);
        }
    }
}


