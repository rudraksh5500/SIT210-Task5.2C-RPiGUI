using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Vector
{
    public class Vector<T> : IEnumerable<T> where T : IComparable<T>
    {
        // This constant determines the default number of elements in a newly created vector.
        // It is also used to extended the capacity of the existing vector
        private const int DEFAULT_CAPACITY = 10;

        // This array represents the internal data structure wrapped by the vector class.
        // In fact, all the elements are to be stored in this private  array. 
        // You will just write extra functionality (methods) to make the work with the array more convenient for the user.
        private T[] data;

        // This property represents the number of elements in the vector
        public int Count { get; private set; } = 0;

        // This property represents the maximum number of elements (capacity) in the vector
        public int Capacity
        {
            get { return data.Length; }
        }

        // This is an overloaded constructor
        public Vector(int capacity)
        {
            data = new T[capacity];
        }

        // This is the implementation of the default constructor
        public Vector() : this(DEFAULT_CAPACITY) { }

        // An Indexer is a special type of property that allows a class or structure to be accessed the same way as array for its internal collection. 
        // For example, introducing the following indexer you may address an element of the vector as vector[i] or vector[0] or ...
        public T this[int index]
        {
            get
            {
                if (index >= Count || index < 0) throw new IndexOutOfRangeException();
                return data[index];
            }
            set
            {
                if (index >= Count || index < 0) throw new IndexOutOfRangeException();
                data[index] = value;
            }
        }

        // This private method allows extension of the existing capacity of the vector by another 'extraCapacity' elements.
        // The new capacity is equal to the existing one plus 'extraCapacity'.
        // It copies the elements of 'data' (the existing array) to 'newData' (the new array), and then makes data pointing to 'newData'.
        private void ExtendData(int extraCapacity)
        {
            T[] newData = new T[Capacity + extraCapacity];
            for (int i = 0; i < Count; i++) newData[i] = data[i];
            data = newData;
        }

        // This method adds a new element to the existing array.
        // If the internal array is out of capacity, its capacity is first extended to fit the new element.
        public void Add(T element)
        {
            if (Count == Capacity) ExtendData(DEFAULT_CAPACITY);
            data[Count++] = element;
        }

        // This method searches for the specified object and returns the zero‐based index of the first occurrence within the entire data structure.
        // This method performs a linear search; therefore, this method is an O(n) runtime complexity operation.
        // If occurrence is not found, then the method returns –1.
        // Note that Equals is the proper method to compare two objects for equality, you must not use operator '=' for this purpose.
        // Method to find the index of an element.
        public int IndexOf(T element)
        {
            for (var i = 0; i < Count; i++)
            {
                if (data[i].Equals(element)) return i;
            }
            return -1;
        }

       
        public ISorter Sorter { set; get; } = new DefaultSorter();

        
        internal class DefaultSorter : ISorter
        {
            public void Sort<K>(K[] sequence, IComparer<K> comparer) where K : IComparable<K>
            {
                if (comparer == null) comparer = Comparer<K>.Default;
                Array.Sort(sequence, comparer);
            }
        }

 
        public void Sort()
        {
            if (Sorter == null) Sorter = new DefaultSorter();
            Array.Resize(ref data, Count);
            Sorter.Sort(data, null);
        }

        
        public void Sort(IComparer<T> comparer)
        {
            if (Sorter == null) Sorter = new DefaultSorter();
            Array.Resize(ref data, Count);
            if (comparer == null) Sorter.Sort(data, null);
            else Sorter.Sort(data, comparer);
        }

        public int BinarySearch(T var)
        {
            IComparer<T> comparer = Comparer<T>.Default;
            return BinarySearch(var, comparer);
        }

      
        public int BinarySearch(T var, IComparer<T> comparer)
        {
            if (Count == 0)
            {
                return -1;
            }
            if (comparer is null)
            {
                comparer = Comparer<T>.Default;
            }
            return BinarySearch(var, comparer, 0, data.Length - 1);
        }

        
        private int BinarySearch(T var, IComparer<T> comparer, int lower, int upper)
        {
            if (lower > upper) return -1;
            int mid = (int)(upper + lower) / 2;
            if (comparer.Compare(var, data[mid]) < 0)
                return BinarySearch(var, comparer, lower, mid - 1);
            else if (comparer.Compare(var, data[mid]) == 0) return mid;
            else return BinarySearch(var, comparer, mid + 1, upper);
        }

        
        public IEnumerator<T> GetEnumerator() => new Iterator(this);

        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

      
        private class Iterator : IEnumerator<T>
        {
            
            private Vector<T> VECTOR;

         
            private int _currentIndex = -1;

            
            public Iterator(Vector<T> v) => VECTOR = v;

            
            public T Current
            {
                get
                {
                    try
                    {
                        
                        return VECTOR.data[_currentIndex];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return default(T);
                    }
                }
            }

            
            object IEnumerator.Current
            {
                get
                {
                    try
                    {
                       
                        return VECTOR.data[_currentIndex];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        
                        return default(T);
                    }
                }
            }

            
            public bool MoveNext()
            {
                
                return ++_currentIndex < VECTOR.Count;
            }

          
            public void Reset() => _currentIndex = -1;

          
            public void Dispose() { }
        }
    }
}


//Tester.cs
using System;
using System.Collections.Generic;

namespace Vector
{

    public class AscendingIntComparer : IComparer<int>
    {
        public int Compare(int A, int B)
        {
            return A - B;
        }
    }

    public class DescendingIntComparer : IComparer<int>
    {
        public int Compare(int A, int B)
        {
            return B - A;
        }
    }

    public class EvenNumberFirstComparer : IComparer<int>
    {
        public int Compare(int A, int B)
        {
            return A % 2 - B % 2;
        }
    }

    public class Student : IComparable<Student>
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public int CompareTo(Student s)
        {
            return this.Id - s.Id;
        }

        public override string ToString()
        {
            return Id + "[" + Name + "]";
        }
        
    }

    class Tester
    {
        private static bool CheckAscending(Vector<int> vector)
        {
            for (int i = 0; i < vector.Count - 1; i++)
                if (vector[i] > vector[i + 1]) return false;
            return true;
        }

        private static bool CheckDescending(Vector<int> vector)
        {
            for (int i = 0; i < vector.Count - 1; i++)
                if (vector[i] < vector[i + 1]) return false;
            return true;
        }

        private static bool CheckEvenNumberFirst(Vector<int> vector)
        {
            for (int i = 0; i < vector.Count - 1; i++)
                if (vector[i]%2 > vector[i + 1]%2) return false;
            return true;
        }
        private static bool CheckSequence<T>(T[] certificate, Vector<T> vector) where T : IComparable<T>
        {
            if (vector == null)
            {
                throw new ArgumentNullException(nameof(vector));
            }
            if (certificate.Length != vector.Count) return false;
            int counter = 0;
            foreach (T value in vector)
            {
                if (!value.Equals(certificate[counter])) return false;
                counter++;
            }
            return true;
        }

        static void Main(string[] args)
        {
            string result = "";
            int problem_size = 20;
            int[] data = new int[problem_size]; data[0] = 333;
            Random k = new Random(1000);
            for (int i = 1; i < problem_size; i++) data[i] = 100+k.Next(900);

            Vector<int> vector = new Vector<int>(problem_size);

            // ------------------ BinarySearch ----------------------------------
            int[] temp = null; int check;

            // test A
            try
            {
                vector.Sorter = null;
                temp = new int[problem_size];
                data.CopyTo(temp, 0);
                Array.Sort(temp, new AscendingIntComparer());
                Console.WriteLine("\nTest A: Apply BinarySearch searching for number 333 to array of integer numbers sorted in AscendingIntComparer: ");
                 vector = new Vector<int>(problem_size);
                for (int i = 0; i < problem_size; i++) vector.Add(data[i]);
                vector.Sort(new AscendingIntComparer());
                Console.WriteLine("Resulting order: " + vector.ToString());
                check = Array.BinarySearch(temp, 333, new AscendingIntComparer());
                check = check < 0 ? -1 : check;
                if (vector.BinarySearch(333, new AscendingIntComparer()) != check) throw new Exception("The resulting index (or return value) is incorrect.");
                Console.WriteLine(" :: SUCCESS");
                result = result + "A";
            }
            catch (Exception exception)
            {
                Console.WriteLine(" :: FAIL");
                Console.WriteLine(exception.ToString());
                result = result + "-";
            }

            // test B
            try
            {
                vector.Sorter = null;
                temp = new int[problem_size];
                data.CopyTo(temp, 0);
                Array.Sort(temp, new AscendingIntComparer());
                Console.WriteLine("\nTest B: Apply BinarySearch searching for number " + (temp[0] - 1) + " to array of integer numbers sorted in AscendingIntComparer: ");
                 vector = new Vector<int>(problem_size);
                for (int i = 0; i < problem_size; i++) vector.Add(data[i]);
                vector.Sort(new AscendingIntComparer());
                Console.WriteLine("Resulting order: " + vector.ToString());
                check = Array.BinarySearch(temp, temp[0] - 1, new AscendingIntComparer());
                check = check < 0 ? -1 : check;
                if (vector.BinarySearch(temp[0] - 1, new AscendingIntComparer()) != check) throw new Exception("The resulting index (or return value) is incorrect.");
                Console.WriteLine(" :: SUCCESS");
                result = result + "B";
            }
            catch (Exception exception)
            {
                Console.WriteLine(" :: FAIL");
                Console.WriteLine(exception.ToString());
                result = result + "-";
            }

            // test C
            try
            {
                vector.Sorter = null;

                Console.WriteLine("\nTest C: Apply BinarySearch searching for number " + (temp[problem_size - 1] + 1) + " to array of integer numbers sorted in AscendingIntComparer: ");
                temp = new int[problem_size];
                data.CopyTo(temp, 0);
                Array.Sort(temp, new AscendingIntComparer());
                 vector = new Vector<int>(problem_size);
                for (int i = 0; i < problem_size; i++) vector.Add(data[i]);
                vector.Sort(new AscendingIntComparer());
                Console.WriteLine("Resulting order: " + vector.ToString());
                check = Array.BinarySearch(temp, temp[problem_size - 1] + 1, new AscendingIntComparer());
                check = check < 0 ? -1 : check;
                if (vector.BinarySearch(temp[problem_size - 1] + 1, new AscendingIntComparer()) != check) throw new Exception("The resulting index (or return value) is incorrect.");
                Console.WriteLine(" :: SUCCESS");
                result = result + "C";
            }
            catch (Exception exception)
            {
                Console.WriteLine(" :: FAIL");
                Console.WriteLine(exception.ToString());
                result = result + "-";
            }

            // test D
            try
            {
                vector.Sorter = null;
                temp = new int[problem_size];
                data.CopyTo(temp, 0);
                Array.Sort(temp, new DescendingIntComparer());
                Console.WriteLine("\nTest D: Apply BinarySearch searching for number 333 to array of integer numbers sorted in DescendingIntComparer: ");
                 vector = new Vector<int>(problem_size);
                for (int i = 0; i < problem_size; i++) vector.Add(data[i]);
                vector.Sort(new DescendingIntComparer());
                Console.WriteLine("Resulting order: " + vector.ToString());
                check = Array.BinarySearch(temp, 333, new DescendingIntComparer());
                check = check < 0 ? -1 : check;
                if (vector.BinarySearch(333, new DescendingIntComparer()) != check) throw new Exception("The resulting index (or return value) is incorrect.");
                Console.WriteLine(" :: SUCCESS");
                result = result + "D";
            }
            catch (Exception exception)
            {
                Console.WriteLine(" :: FAIL");
                Console.WriteLine(exception.ToString());
                result = result + "-";
            }

            // test E
            try
            {
                vector.Sorter = null;
                temp = new int[problem_size];
                data.CopyTo(temp, 0);
                Array.Sort(temp, new DescendingIntComparer());
                Console.WriteLine("\nTest E: Apply BinarySearch searching for number " + (temp[0] - 1) + " to array of integer numbers sorted in DescendingIntComparer: ");
                 vector = new Vector<int>(problem_size);
                for (int i = 0; i < problem_size; i++) vector.Add(data[i]);
                vector.Sort(new DescendingIntComparer());
                Console.WriteLine("Resulting order: " + vector.ToString());
                check = Array.BinarySearch(temp, temp[0] - 1, new DescendingIntComparer());
                check = check < 0 ? -1 : check;
                if (vector.BinarySearch(temp[0] - 1, new DescendingIntComparer()) != check) throw new Exception("The resulting index (or return value) is incorrect.");
                Console.WriteLine(" :: SUCCESS");
                result = result + "E";
            }
            catch (Exception exception)
            {
                Console.WriteLine(" :: FAIL");
                Console.WriteLine(exception.ToString());
                result = result + "-";
            }

            // test F
            try
            {
                vector.Sorter = null;

                Console.WriteLine("\nTest F: Apply BinarySearch searching for number " + (temp[problem_size - 1] + 1) + " to array of integer numbers sorted in DescendingIntComparer: ");
                temp = new int[problem_size];
                data.CopyTo(temp, 0);
                Array.Sort(temp, new DescendingIntComparer());
                 vector = new Vector<int>(problem_size);
                for (int i = 0; i < problem_size; i++) vector.Add(data[i]);
                vector.Sort(new DescendingIntComparer());
                Console.WriteLine("Resulting order: " + vector.ToString());
                check = Array.BinarySearch(temp, temp[problem_size - 1] + 1, new DescendingIntComparer());
                check = check < 0 ? -1 : check;
                if (vector.BinarySearch(temp[problem_size - 1] + 1, new DescendingIntComparer()) != check) throw new Exception("The resulting index (or return value) is incorrect.");
                Console.WriteLine(" :: SUCCESS");
                result = result + "F";
            }
            catch (Exception exception)
            {
                Console.WriteLine(" :: FAIL");
                Console.WriteLine(exception.ToString());
                result = result + "-";
            }

            

            // test G
            try
            {
                Console.WriteLine("\nTest G: Run a sequence of operations: ");
                Console.WriteLine("Create a new vector by calling 'Vector<int> vector = new Vector<int>(5);'");
                vector = new Vector<int>(5);
                Console.WriteLine(" :: SUCCESS");
                Console.WriteLine("Add a sequence of numbers 2, 6, 8, 5, 5, 1, 8, 5, 3, 5, 7, 1, 4, 9");
                vector.Add(2); vector.Add(6); vector.Add(8); vector.Add(5); vector.Add(5); vector.Add(1); vector.Add(8); vector.Add(5);
                vector.Add(3); vector.Add(5); vector.Add(7); vector.Add(1); vector.Add(4); vector.Add(9);
                Console.WriteLine(" :: SUCCESS");
                result = result + "G";
            }
            catch (Exception exception)
            {
                Console.WriteLine(" :: FAIL");
                Console.WriteLine(exception.ToString());
                result = result + "-";
            }

            // test H
            try
            {
                Console.WriteLine("\nTest H: Run a sequence of operations: ");
                Console.WriteLine("Check whether the interface IEnumerable<T> is implemented for the Vector<T> class");
                if (!(vector is IEnumerable<int>)) throw new Exception("Vector<T> does not implement IEnumerable<T>");
                Console.WriteLine(" :: SUCCESS");
                Console.WriteLine("Check whether GetEnumerator() method is implemented");
                vector.GetEnumerator();
                Console.WriteLine(" :: SUCCESS");
                result = result + "H";
            }
            catch (NotImplementedException)
            {
                Console.WriteLine(" :: FAIL");
                Console.WriteLine("GetEnumerator() method is not implemented");
                result = result + "-";
            }
            catch (Exception exception)
            {
                Console.WriteLine(" :: FAIL");
                Console.WriteLine(exception.ToString());
                result = result + "-";
            }

            // test I
            try
            {
                Console.WriteLine("\nTest I: Run a sequence of operations: ");
                Console.WriteLine("Return the Enumerator of the Vector<T> and check whether it implements IEnumerator<T>");
                if (!(vector.GetEnumerator() is IEnumerator<int>)) throw new Exception("The Enumerator of the Vector<T> does not implement IEnumerator<T>");
                Console.WriteLine("Check the initial value of Current of the Enumerator");
                if (vector.GetEnumerator().Current != default(int)) throw new Exception("The initial Current value of the Enumerator is incorrect. Must be the value of " + default(int));

                Console.WriteLine("Check the value of Current of the Enumerator after MoveNext() operation");
                IEnumerator<int> enumerator = vector.GetEnumerator();
                enumerator.MoveNext();
                if (enumerator.Current != 2) throw new Exception("The value of Current of the Enumerator after MoveNext() operation is incorrect. Must be the value of " + vector[0]);
                Console.WriteLine(" :: SUCCESS");
                result = result + "I";
            }
            catch (Exception exception)
            {
                Console.WriteLine(" :: FAIL");
                Console.WriteLine(exception.ToString());
                result = result + "-";
            }

            // test J
            try
            {
                Console.WriteLine("\nTest J: Check the content of the Vector<int> by traversing it via 'foreach' statement ");
                if (!CheckSequence(new int[] { 2, 6, 8, 5, 5, 1, 8, 5, 3, 5, 7, 1, 4, 9 }, vector)) throw new Exception("The 'foreach' statement produces an incorrect sequence of integers");
                Console.WriteLine(" :: SUCCESS");
                result = result + "J";
            }
            catch (Exception exception)
            {
                Console.WriteLine(" :: FAIL");
                Console.WriteLine(exception.ToString());
                result = result + "-";
            }

            // test K
            int num = 14;
            Student[] certificate_student = new Student[num];
            Vector<Student> students = null;
            try
            {
                string[] names = new string[] { "Kelly", "Cindy", "John", "Andrew", "Richard", "Michael", "Guy", "Elicia", "Tom", "Iman", "Simon", "Vicky" };
                Random random = new Random(100);
                Console.WriteLine("\nTest K: Run a sequence of operations: ");
                Console.WriteLine("Create a new vector of Student objects by calling 'Vector<Student> students = new Vector<Student>();'");
                students = new Vector<Student>();
                for (int i = 0; i < num; i++)
                {
                    Student student = new Student() { Name = names[random.Next(0, names.Length)], Id = i };
                    Console.WriteLine("Add student with record: " + student.ToString());
                    students.Add(student);
                    certificate_student[i] = student;
                }
                Console.WriteLine("Print the vector of students via students.ToString();");
                Console.WriteLine(students.ToString());

                Console.WriteLine(" :: SUCCESS");
                result = result + "K";
            }
            catch (Exception exception)
            {
                Console.WriteLine(" :: FAIL");
                Console.WriteLine(exception.ToString());
                result = result + "-";
            }

            // test J
            try
            {
                Console.WriteLine("\nTest J: Check the content of the Vector<Student> by traversing it via 'foreach' statement ");
                if (!CheckSequence(certificate_student, students)) throw new Exception("The 'foreach' statement produces an incorrect sequence of students");
                Console.WriteLine(" :: SUCCESS");
                result = result + "J";
            }
            catch (Exception exception)
            {
                Console.WriteLine(" :: FAIL");
                Console.WriteLine(exception.ToString());
                result = result + "-";
            }*/

            Console.WriteLine("\n\n ------------------- SUMMARY ------------------- ");
            Console.WriteLine("Tests passed: " + result);
            Console.ReadKey();

        }
    }
}


//ISorter.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vector
{
    public interface ISorter
    {
        void Sort<K>(K[] sequence, IComparer<K> comparer) where K : IComparable<K>;
    }
}
