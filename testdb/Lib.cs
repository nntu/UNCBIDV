using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UyNhiemChiBIDV
{
    public static class Lib
    {
        public static string[] SplitWords(string s)
        {
            //
            // Split on all non-word characters.
            // ... Returns an array of all the words.
            //
            return Regex.Split(s, @"\W+");
            // @      special verbatim string syntax
            // \W+    one or more non-word characters together
        }


        public static string[] ChiaDong (string stringToSplit, int sochu) {
            string[] re = new string[2];
            string[] words = stringToSplit.Split(' ');
            StringBuilder line1 = new StringBuilder(); StringBuilder line2 = new StringBuilder();
            int i = 0;
            foreach (string word in words)
            {
                if (i <= sochu)
                {
                    line1.Append(word + " ");
                }
                else {
                    line2.Append(word + " ");
                }
                i++;
            }
            re[0]= line1.ToString().Trim();
            re[1] = line2.ToString().Trim();
             return re;
        }

        public static IEnumerable<string> SplitToLines(string stringToSplit, int maxLineLength)
        {
            string[] words = stringToSplit.Split(' ');
            StringBuilder line = new StringBuilder();
            foreach (string word in words)
            {
                if (word.Length + line.Length <= maxLineLength)
                {
                    line.Append(word + " ");
                }
                else
                {
                    if (line.Length > 0)
                    {
                        yield return line.ToString().Trim();
                        line.Clear();
                    }
                    string overflow = word;
                    //while (overflow.Length > maxLineLength)
                    //{
                    //    yield return overflow.Substring(0, maxLineLength);
                    //    overflow = overflow.Substring(maxLineLength);
                    //}
                    line.Append(overflow + " ");
                }
            }
            yield return line.ToString().Trim();
        }


        public static int GetNoOfWords(string s)
        {
            return s.Split(new char[] { ' ', '.', ',', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        public static string DocSoThanhChu(string number)//<=
        {
            string strReturn = "";
            string s = number;
            while (s.Length > 0 && s.Substring(0, 1) == "0")
            {
                s = s.Substring(1);
            }
            string[] so = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] hang = new string[] { "", "nghìn", "triệu", "tỷ" };
            int i, j, donvi, chuc, tram;

            bool booAm = false;
            decimal decS = 0;

            try
            {
                decS = Convert.ToDecimal(s.ToString());
            }
            catch { }

            if (decS < 0)
            {
                decS = -decS;
                //s = decS.ToString();
                booAm = true;
            }
            i = s.Length;
            if (i == 0)
                strReturn = so[0] + strReturn;
            else
            {
                j = 0;
                while (i > 0)
                {
                    donvi = Convert.ToInt32(s.Substring(i - 1, 1));
                    i--;
                    if (i > 0)
                        chuc = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        chuc = -1;
                    i--;
                    if (i > 0)
                        tram = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        tram = -1;
                    i--;
                    if ((donvi > 0) || (chuc > 0) || (tram > 0) || (j == 3))
                        strReturn = hang[j] + strReturn;
                    j++;
                    if (j > 3) j = 1;   //Tránh lỗi, nếu dưới 13 số thì không có vấn đề.
                                        //Hàm này chỉ dùng để đọc đến 9 số nên không phải bận tâm
                    if ((donvi == 1) && (chuc > 1))
                        strReturn = "mốt " + strReturn;
                    else
                    {
                        if ((donvi == 5) && (chuc > 0))
                            strReturn = "lăm " + strReturn;
                        else if (donvi > 0)
                            strReturn = so[donvi] + " " + strReturn;
                    }
                    if (chuc < 0) break;//Hết số
                    else
                    {
                        if ((chuc == 0) && (donvi > 0)) strReturn = "linh " + strReturn;
                        if (chuc == 1) strReturn = "mười " + strReturn;
                        if (chuc > 1) strReturn = so[chuc] + " mươi " + strReturn;
                    }
                    if (tram < 0) break;//Hết số
                    else
                    {
                        if ((tram > 0) || (chuc > 0) || (donvi > 0)) strReturn = so[tram] + " trăm " + strReturn;
                    }
                    strReturn = " " + strReturn;
                }
            }
            if (booAm) strReturn = "Âm " + strReturn;
            return strReturn.Trim();// = str+ "đồng chẵn";
        }

        private static string PhanCach(string number)
        {
            var sb = "";
            var split = number.Trim().Split('.').ToList();
            for (var i = 0; i < split.Count; i++)
            {
                if (i > 0)
                    sb += " lẻ ";
                sb += ChuyenSo(split[i]);
            }
            sb += " đồng";

            return Regex.Replace(sb.Substring(0, 1).ToUpper() + sb.Substring(1).ToLower(), @"\s+", " ");
        }

        public static string ChuyenSo(string number)
        {
            string[] dv = { "", "mươi", "trăm", "nghìn", "triệu", "tỉ" };
            string[] cs = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };

            var length = number.Length;
            number += "ss";
            var doc = new StringBuilder();
            var rd = 0;

            var i = 0;
            while (i < length)
            {
                //So chu so o hang dang duyet
                var n = (length - i + 2) % 3 + 1;

                //Kiem tra so 0
                var found = 0;
                int j;
                for (j = 0; j < n; j++)
                {
                    if (number[i + j] == '0') continue;
                    found = 1;
                    break;
                }

                //Duyet n chu so
                int k;
                if (found == 1)
                {
                    rd = 1;
                    for (j = 0; j < n; j++)
                    {
                        var ddv = 1;
                        switch (number[i + j])
                        {
                            case '0':
                                if (n - j == 3)
                                    doc.Append(cs[0]);
                                if (n - j == 2)
                                {
                                    if (number[i + j + 1] != '0')
                                        doc.Append("lẻ");
                                    ddv = 0;
                                }
                                break;

                            case '1':
                                switch (n - j)
                                {
                                    case 3:
                                        doc.Append(cs[1]);
                                        break;

                                    case 2:
                                        doc.Append("mười");
                                        ddv = 0;
                                        break;

                                    case 1:
                                        k = (i + j == 0) ? 0 : i + j - 1;
                                        doc.Append((number[k] != '1' && number[k] != '0') ? "mốt" : cs[1]);
                                        break;
                                }
                                break;

                            case '5':
                                doc.Append((i + j == length - 1) ? "lăm" : cs[5]);
                                break;

                            default:
                                doc.Append(cs[number[i + j] - 48]);
                                break;
                        }

                        doc.Append(" ");

                        //Doc don vi nho
                        if (ddv == 1)
                            doc.Append(dv[n - j - 1] + " ");
                    }
                }

                //Doc don vi lon
                if (length - i - n > 0)
                {
                    if ((length - i - n) % 9 == 0)
                    {
                        if (rd == 1)
                            for (k = 0; k < (length - i - n) / 9; k++)
                                doc.Append("tỉ ");
                        rd = 0;
                    }
                    else
                        if (found != 0) doc.Append(dv[((length - i - n + 1) % 9) / 3 + 2] + " ");
                }

                i += n;
            }

            return (length == 1) && (number[0] == '0' || number[0] == '5') ? cs[number[0] - 48] : doc.ToString();
        }
    }
}