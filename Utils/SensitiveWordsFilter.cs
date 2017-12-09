using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
namespace Com.Yst.Framework.Utils
{
    /// <summary>
    /// �������д�
    /// </summary>
    public class SensitiveWordsFilter
    {
        private HashSet<string> hash = new HashSet<string>();
        private byte[] fastCheck = new byte[char.MaxValue];
        private byte[] fastLength = new byte[char.MaxValue];

        private BitArray charCheck = new BitArray(char.MaxValue);
        private BitArray endCheck = new BitArray(char.MaxValue);
        private int maxWordLength = 0;
        private int minWordLength = int.MaxValue;


        public void Init(string[] badwords)
        {
            foreach (string word in badwords)
            {
                if (string.IsNullOrEmpty(word))
                {
                    continue;
                }
                maxWordLength = Math.Max(maxWordLength, word.Length);
                minWordLength = Math.Min(minWordLength, word.Length);

                for (int i = 0; i < 7 && i < word.Length; i++)
                {
                    fastCheck[word[i]] |= (byte)(1 << i);
                }

                for (int i = 7; i < word.Length; i++)
                {
                    fastCheck[word[i]] |= 0x80;
                }

                if (word.Length == 1)
                {
                    charCheck[word[0]] = true;
                }
                else
                {
                    fastLength[word[0]] |= (byte)(1 << (Math.Min(7, word.Length - 2)));
                    endCheck[word[word.Length - 1]] = true;

                    hash.Add(word);
                }
            }
        }

        public string Filter(string text, string mask)
        {
            throw new NotImplementedException();
        }

        public string[] HasBadWord(string text)
        {
            List<string> badKeyword = new List<string>();
            int index = 0;

            while (index < text.Length)
            {
                int count = 1;

                if (index > 0 || (fastCheck[text[index]] & 1) == 0)
                {
                    while (index < text.Length - 1 && (fastCheck[text[++index]] & 1) == 0) ;
                }

                char begin = text[index];

                if (minWordLength == 1 && charCheck[begin])
                {
                    badKeyword.Add("" + begin);
                    //return true;
                }

                for (int j = 1; j <= Math.Min(maxWordLength, text.Length - index - 1); j++)
                {
                    char current = text[index + j];

                    if ((fastCheck[current] & 1) == 0)
                    {
                        ++count;
                    }

                    if ((fastCheck[current] & (1 << Math.Min(j, 7))) == 0)
                    {
                        break;
                    }

                    if (j + 1 >= minWordLength)
                    {
                        if ((fastLength[begin] & (1 << Math.Min(j - 1, 7))) > 0 && endCheck[current])
                        {
                            string sub = text.Substring(index, j + 1);

                            if (hash.Contains(sub))
                            {
                                badKeyword.Add(sub);
                                //return true;
                            }
                        }
                    }
                }

                index += count;
            }

            //return false;
            return badKeyword.ToArray();
        }
    }
}