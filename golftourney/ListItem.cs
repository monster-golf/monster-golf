using System;

namespace MonsterGolf
{
   /// <summary>
   /// Summary description for ListItem.
   /// </summary>
   public class ListItem
   {
      private string m_id;
      private string m_name;

      public ListItem(string id, string name) 
      {
         m_id = id;
         m_name = name;
      }
      public string ID 
      {
         get { return m_id; }
      }
      public string Name
      {
         get { return m_name; }
      }
      public override string ToString()
      {
         return m_name;
      }
   }
}
