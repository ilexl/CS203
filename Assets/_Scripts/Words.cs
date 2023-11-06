using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

    public List<string> ConvertCharBoardToWords(List<List<char>> raw)
    {
        return ExtractStrings(raw);
    }

    List<string> ExtractStrings(List<List<char>> lettersGrid)
    {
        int numRows = lettersGrid.Count;
        int numCols = lettersGrid[0].Count;
        List<string> returnList = new List<string>();

        for (int row = 0; row < numRows; row++)
        {
            string horizontalString = "";
            for (int col = 0; col < numRows; col++)
            {
                horizontalString += lettersGrid[row][col];
            }

            List<string> indivWords = horizontalString.Split(' ').ToList();
            int coll = 0;
            foreach (string word in indivWords)
            {
                if (word != " " && word != null && word != "")
                {
                    if (word.Length < 2)
                    {
                        if (CheckSingle(lettersGrid, word, row, coll))
                        {
                            if (!returnList.Contains(word))
                            {
                                returnList.Add(word);
                            }
                        }
                    }
                    else { returnList.Add(word); }
                }
                coll += word.Length;
                if(word.Length == 0) { coll++; }
            }
        }

        /*
        foreach (List<char> row in lettersGrid)
        {
            // split into list with positions still accurate - comes with white space
            string horizontalString = new string(row.ToArray());
            List<string> indivWords = horizontalString.Split(' ').ToList(); 
            foreach(string word in indivWords)
            {
                if(word != " " && word != null && word != "")
                {
                    returnList.Add(word);
                }

            }
        }*/

        

        for (int col = 0; col < numCols; col++)
        {
            string verticalString = "";
            for (int row = 0; row < numRows; row++)
            {
                verticalString += lettersGrid[row][col];
            }

            int roww = 0;
            List<string> indivWords = verticalString.Split(' ').ToList();
            foreach (string word in indivWords)
            {
                if (word != " " && word != null && word != "")
                {
                    if (word.Length < 2) 
                    { 
                        if(CheckSingle(lettersGrid, word, roww, col))
                        {
                            if (!returnList.Contains(word))
                            {
                                returnList.Add(word);
                            }
                        } 
                    }
                    else 
                    { 
                        returnList.Add(word); 
                    }
                }
                roww += word.Length;
                if (word.Length == 0) { roww++; }
            }
        }

        return returnList;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="raw"></param>
    /// <param name="word"></param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns>true if single letter else false if part of a word</returns>
    bool CheckSingle(List<List<char>> raw, string word, int row, int col)
    {
        int numRows = raw.Count;
        int numCols = raw[0].Count;
        bool isSingle = true;

        // Check above
        if (row > 0 && raw[row - 1][col] != ' ')
        {
            char c = raw[row - 1][col];
            if(c != ' ') { isSingle = false; }
        }

        // Check below
        if (row < numRows - 1 && raw[row + 1][col] != ' ')
        {
            char c = raw[row + 1][col];
            if (c != ' ') { isSingle = false; }
        }

        // Check left
        if (col > 0 && raw[row][col - 1] != ' ')
        {
            char c = raw[row][col - 1];
            if (c != ' ') { isSingle = false; }
        }

        // Check right
        if (col < numCols - 1 && raw[row][col + 1] != ' ')
        {
            char c = raw[row][col + 1];
            if (c != ' ') { isSingle = false; }
        }

        return isSingle;
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(Words))]
public class EDITOR_Words : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Test"))
        {
            Words words = (Words)target;
            words.ConvertCharBoardToWords(words.GetComponent<Game>().lettersGrid);
        }
    }
}
#endif

