 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knapsack

{
    
   public  class Tools
    {
        public List<int> getSelectedItems()
        {
            return selectedItems;
        }
        private Dictionary<Key,Solution> myDictionary = new Dictionary<Key,Solution>();
        //private Dictionary<Key,int> bestScores = new Dictionary<Key,int>();
        private List<int> selectedItems;
        private int counter;
        private int oracle1;
        private int greedyoracle;
        public int getGreedyoracle()
        {
            return greedyoracle;
        }
        public int getOracle1()
        {
            return oracle1;
        }
        public int getCounter()
        {
            return counter;
        }
        public int layer;
        public int getLayer()
        {
            return layer;
        }
        //private Dictionary<int> myDictionary;
        private List<int[]> orderedItems = new List<int[]>();
        public List<int[]> getOrdered()
        {
            return orderedItems;
        }
        private String myString;
        private List<int[]> values;
        public List<int[]> getValues()
        {
            return values;
        }
        private List<double[]> valueperweight;
        public List<double[]> getValueperweight()
        {
            return valueperweight;
        }
        private int bestItem;
       
        private Solution ora;
        public Solution getOra()
        {
            return ora;
        }

        private Solution greedyora;
        public Solution getGreedyora()
        {
            return greedyora;
        }

        private Solution idealora;
        public Solution getIdealora()
        {
            return idealora;
        }
        private int[] valueAchieved;
        private int totalCapacity;
        private int numberItems;

        public Tools(String myString,int capacity,int numItems){
            numberItems = numItems;
            totalCapacity = capacity;
            valueAchieved = new int[capacity];
            
            selectedItems = new List<int>();
            layer = 0;
            this.myString = myString;
            items();
            makeVperw();
            //display of solutions
            idealora = new Solution(ideal(totalCapacity, numberItems-1));

            greedyora = new Solution(GreedyOracle(capacity,numberItems-1,0));
            greedyoracle = greedyora.getTotalValue();
            Console.ReadKey();
            // ora = new Solution(oracle(capacity, numItems-1 ));
            // oracle1 = ora.getTotalValue();
        }
        //we create a list with the value/weight ratio of the objects.-------------------------------------------------------
        public void makeVperw()
        {
            valueperweight = new List<double[]>();
            for (int i = 0; i < values.Count; i++)
            {

                double x = (double)values[i][0] / values[i][1];
                x = Math.Round(x,15);
                double index = (double)i;
                double weight = (double)values[i][1];
                double value = (double)values[i][0];
                double[] kati = { -x,index,weight,value};
                
                valueperweight.Add(kati);

            }

            var result = valueperweight.OrderBy(e => e[0]).ThenBy(e => e[2]);
            counter = 0;
            foreach(double[] a in result)
            {
                valueperweight[counter] = a;
                counter++;
            }

            foreach (double[] a in valueperweight)
            {
                orderedItems.Add(new int[]{(int)a[3],(int)a[2]});
                  
            }
            counter = 0;
            foreach (double[] a in valueperweight)
            {
                values[numberItems-1-counter][0] =(int) a[3];
                values[numberItems-1-counter][1] =(int) a[2];
                counter++;
            }
            //int[] temp;
            //temp = new int[] { values[0][0], values[0][1] };
            //values[0][0] = values[29][0];
            //values[0][1] = values[29][1];
            //values[29][0] = temp[0];
            //values[29][1] = temp[1];
            counter = 0;
            foreach (int[] a in getValues())
            {
                Console.WriteLine(" index " + counter + " value " + a[0] + " weight " + a[1]);
                counter++;
            }

        }
    
        // GREEDY SOLUTION IS PRODUCED -----------------------------------------------------------------
        public Solution GreedyOracle(int k,int j,int greedylayer)
        {
            //Key key = new Key(k, numberItems - j - 1);
            //if (myDictionary.ContainsKey(key))
            //{
            //    Solution sol;
            //    bool found = myDictionary.TryGetValue(key, out sol);
            //    layer++;
            //    return sol;
            //}
            greedylayer++;

            int value = 0;
            int capacity = k;
            List<int> greedyitems = new List<int>();
            for(int i = numberItems-j-1;i<orderedItems.Count;i++)
            {
                

                if (greedylayer <2)
                {
                
                    Solution withItem = GreedyOracle(capacity, numberItems - i - 1, greedylayer);

                    Solution withoutItem = GreedyOracle(capacity, numberItems - i - 2, greedylayer);

                    //Key keywithItem = new Key(capacity, numberItems - i - 1);
                    //if (!myDictionary.ContainsKey(keywithItem))
                    //{
                    //    myDictionary.Add(keywithItem, withItem);
                    //}

                    //Key keyWithout = new Key(capacity, numberItems - i - 2);
                    //if (!myDictionary.ContainsKey(keyWithout))
                    //{
                    //    myDictionary.Add(keyWithout, withoutItem);
                    //}
                    if (withItem.getTotalValue() <= withoutItem.getTotalValue())
                    {
                        continue;
                    }
                 
                    //else
                    //{
                    //    for (int it = 0; it < greedyitems.Count; it++)
                    //    {
                    //        withItem.addItem(greedyitems[j]);
                    //    }
                    //    withItem.setTotalValue(withItem.getTotalValue() + value);
                    //    return withItem;
                    //}
                }
             
                if (capacity >= orderedItems[i][1])
                {
                    value += orderedItems[i][0];
                    capacity = capacity - orderedItems[i][1];
                    greedyitems.Add(i);
                   
                }
                if (capacity <= 0)
                {
                    break;
                   
                }
                
            }
            return new Solution(value, greedyitems);
           
          
        }
        public int maxweight()
        {
            int max = 0;
            foreach(int[] a in values)
            {
                if (a[1] > max)
                    max = a[1];
            }
            return max;
        }
   
        // we make the list of items from string to int array 
        public void items()
        {
            List<String[]> itemsList = new List<String[]>();
            String[] items = myString.Split(new[] { '\r', '\n' });
            for (int i = 1; i < items.Length; i++)
            {
                String[] x = items[i].Split(' ');

                itemsList.Add(x);
            }
             values = new List<int[]>();
            for (int j = 0; j < itemsList.Count -1; j++)
            {

                int[] x = new int[2];
                x[0] = Convert.ToInt32(itemsList[j][0]);

                x[1] = Convert.ToInt32(itemsList[j][1]);
               // Console.WriteLine("x[0] " + x[0] + " x[1] " + x[1]);
                values.Add(x);

            }    
        }
        // we find the item with the best value/weight ratio
        public int bestRatio(List<double> list)
        {
            double max = -1;
            int maxint = -1;
            for (int i = 0; i < list.Count; i++)
            {
                if (max <= list[i])
                {
                    max = list[i];
                    maxint = i;

                }
            }
            return maxint;
        }
        //IDEAL SOLUTSION --------------------------------------------------------------
        public Solution ideal(int k,int j)
        {
           
            int value = 0;
            int capacity = k;
            List<int> idealitems = new List<int>();
            for(int i= numberItems-j-1;i<orderedItems.Count;i++)
            {
                if (capacity >= orderedItems[i][1])
                {
                    value += orderedItems[i][0];
                    capacity = capacity - orderedItems[i][1];
                    idealitems.Add(i);
            
                }
                else
                {
                    double c = (double)capacity;
                    double o = (double)orderedItems[i][1];
                    double portion = c / o;
                   // Console.WriteLine(portion);
                    double portionalvalue = portion * orderedItems[i][0];
                   // Console.WriteLine(portionalvalue);
                    value = value + (int)portionalvalue;
                   // Console.WriteLine(value);
                    idealitems.Add(i);
                    return new Solution(value, idealitems);
                }
            }
            return new Solution(value, idealitems);
        }

        private int pruned=0;
        public int getPruned()
        {
            return pruned;
        }
        public int weightLeft(int j)
        {
            int w=0;
            for (int i = numberItems - j-1; i < orderedItems.Count; i++)
            {
                w = w + orderedItems[i][1];
            }
            return w;
        }

        public int valueLeft(int j)
        {
            int v = 0;
            for (int i = numberItems - j-1; i < orderedItems.Count; i++)
            {
                v = v + orderedItems[i][0];
            }
            return v;
        }



        //ORACLE---------------------------------------------------------------------------------------
        public Solution oracle(int k, int j )
        {
            
            //Dictionary -------------------------
            Key key = new Key(k, j);
            if (myDictionary.ContainsKey(key))
            {
                Solution sol;
                bool found = myDictionary.TryGetValue(key, out sol);
           
                layer++; 
                
                return sol;
            }
            if (myDictionary.Count > 5000000)
            {
                foreach (var kw in myDictionary.Keys.OrderBy(kwkey => 1).Take(100000)) myDictionary.Remove(kw);
            }
            // end dictionary -----------------------

            //we create greedy solution to use as heuristic-------


            //if (bestScores.ContainsKey(key))
            //{
            //    bestScores.TryGetValue(key, out currentValue);
            //    if (currentValue < greedyscore)
            //    {
            //        bestScores[key] = greedyscore;
            //    }
            //}else
            //{
            //    bestScores.Add(key, greedyscore);
            //}

            // end of heuristic------------------------------------
            if (j == -1)
            {
                Solution sol = new Solution(0);
                return sol;
            }
            else if (values[j][1] <= k)
            {
                Solution lamda;
                Solution kapa;
                if (weightLeft(j) <= k) {
                  
                    List<int> list = new List<int>();
                    for (int i = numberItems - j-1; i < values.Count; i++)
                    {
                        list.Add(i);
                    }
                    return new Solution(valueLeft(j), list);
                }
                //bestScores.TryGetValue(key,out currentValue);
                
                if (GreedyOracle(k, j,0).getTotalValue() >= ideal(k, j - 1).getTotalValue())
                {
                    pruned++;
                    if (pruned % 10000000 == 0)
                        Console.WriteLine("pruned " + pruned);
                    
                    kapa = new Solution(oracle(k - values[j][1], j - 1));
                   
                    kapa.setTotalValue(kapa.getTotalValue() + values[j][0]);
                    kapa.addItem(j);
                   
                    //lamda = new Solution(oracle(k, j - 1));

                    //if (lamda.getTotalValue() > kapa.getTotalValue())
                    //{
                       
                    //    Console.WriteLine("ERROR ERROR " + " k " + k + " numerItems-j " + (numberItems - j)+" j "+j);
                    //    Console.WriteLine("k-values[j][1] " + (k - values[j][1]) + " j-1 " + (j - 1));
                    //    Console.WriteLine("kati " + kati + " value[j][0] " + vas +" value[j][1] "+values[j][1]);
                    //    Console.Write(" kapa items ");
                    //    for (int i = 0; i < kapa.getItems().Count; i++)
                    //    {
                    //        Console.Write(" n " + kapa.getItems()[i] );
                    //    }
                    //    Console.WriteLine();
                        
                    //    Console.WriteLine("greedyscore " + GreedyOracle(k, j).getTotalValue() + " ideal " + ideal(k, j - 1).getTotalValue() + " kapa " + kapa.getTotalValue() + " lamda " + lamda.getTotalValue());
                    //    for(int i = 0; i < GreedyOracle(k, j).getItems().Count; i++)
                    //    {
                    //        Console.Write(" num "+GreedyOracle(k, j).getItems()[i]);
                    //    }
                    //    for (int i = 0; i < kapa.getItems().Count; i++)
                    //    {
                    //        Console.Write(" kapa num " + kapa.getItems()[i]);
                    //    }
                    //    Console.ReadKey();
                    //}
                    myDictionary.Add(key, kapa);
                    return kapa;

                }
               
                kapa = new Solution(oracle(k - values[j][1], j - 1));
                kapa.setTotalValue(kapa.getTotalValue() + values[j][0]);
                kapa.addItem(j);

                lamda = new Solution( oracle(k, j - 1));
             
                //Key lamdaKey = new Key(k, j-1);
                if (lamda.getTotalValue() > kapa.getTotalValue())
                {
                    if (!myDictionary.ContainsKey(key))
                    {
                        myDictionary.Add(key, lamda);
                    }
                    return lamda;
                }else
                {
                    myDictionary.Add(key, kapa);
                    return kapa;
                }
                
               
            }
            else
            {
                Key lamdaKey = new Key(k, j-1);
                Solution zeta = new Solution(oracle(k, j-1));
                if (!myDictionary.ContainsKey(lamdaKey))
                {
                    myDictionary.Add(lamdaKey, zeta);
                }
           
                return zeta;
            }
        }
       

    }

    class Program
    {
        static void Main(string[] args)
        {
           
            System.IO.StreamReader myFile = new System.IO.StreamReader("C:\\Users\\chara\\Desktop\\Knapsack\\ks10000.txt");
            string myString = myFile.ReadToEnd();

            myFile.Close();
            Tools tool = new Tools(myString, 1000000, 10000);
            foreach (double[] a in tool.getValueperweight())
            {
                Console.WriteLine("valuePerweight "+a[0]+" index "+a[1] +" weight "+a[2]);
            }
            Console.WriteLine();
            Console.WriteLine("ideal value "+tool.getIdealora().getTotalValue());
            for (int i = 0; i < tool.getIdealora().getItems().Count; i++)
            {
                Console.Write(" Ideal items " + tool.getIdealora().getItems()[i] + " " + tool.getOrdered()[tool.getIdealora().getItems()[i]][0]);
            }
            Console.WriteLine();
            Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine("dictionary hits  " + tool.getLayer());
            Console.WriteLine("greedyoracle " + tool.getGreedyoracle());
            for(int i = 0; i < tool.getGreedyora().getItems().Count; i++)
            {
                Console.Write(" Greedy items "+tool.getGreedyora().getItems()[i] + " " + tool.getOrdered()[tool.getGreedyora().getItems()[i]][0]);
            }
            Console.WriteLine();
            Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine("pruned "+tool.getPruned());
        
            Console.WriteLine("oracle1 " + tool.getOracle1());
            Console.WriteLine("dictionary hits " + tool.getLayer());
            Console.WriteLine("tool.getOra().getItems().Count " + tool.getOra().getItems().Count);
            for (int i = 0; i < tool.getOra().getItems().Count; i++)
            {
                Console.Write(" n " + tool.getOra().getItems()[i] + " v  " + tool.getValues()[tool.getOra().getItems()[i]][0]);
            }

            Console.ReadKey();
            System.Threading.Thread.Sleep(5000);
        }
         
    }
    class Key
    {
        private int k;
        private int j;

        public Key(int k, int j)
        {
            this.k = k;
            this.j = j;
        }
        public bool Equals(Key other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return k == other.k && j == other.j;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Key)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (k * 397) ^ j;
            }
        }

        public static bool operator ==(Key left, Key right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Key left, Key right)
        {
            return !Equals(left, right);
        }
    }
 
    public class Solution
    {
        public Solution( int value , List<int> sitems)
        {
            totalValue = value;
            items = new List<int>(sitems);
        }
        public Solution(Solution sol)
        {
            this.totalValue = sol.getTotalValue();
            items = new List<int>();
            setItems(sol.getItems());
        }

        public Solution(int totalValue)
        {
            this.totalValue = totalValue;
            
        }
        private int totalValue;
        public int getTotalValue()
        {
            return totalValue;  
        }
        public void setTotalValue(int kati)
        {
            totalValue = kati; 
        }
        private List<int> items;
        public List<int> getItems()
        {
            return items;
        }
        public void setItems(List<int> itemz)
        {
            
            if (itemz == null)
                return;

            items = new List<int>(itemz);
        }
        public void addItem(int j)
        {
           
            items.Add(j);
        }
    }
}
        
    

