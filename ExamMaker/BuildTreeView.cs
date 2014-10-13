using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;

namespace ExamMaker
{
    class BuildTreeView
    {
        public void BuildTree(XmlReader reader, TreeViewItem TreeViewItem)
        {
            // TreeViewItem to add to existing tree
            TreeViewItem newNode = new TreeViewItem();
            while (reader.Read())
            {
                // build tree based on node type
                switch (reader.NodeType)
                {
                    // if Text node, add its value to tree
                    case XmlNodeType.Text:
                        newNode.Header = reader.Value;
                        TreeViewItem.Items.Add(newNode);
                        break;
                    case XmlNodeType.EndElement: // if EndElement, move up tree
                        if ((reader.Name == "Questi")) // rather than showing the Questi Element i decided to show only the TextNode inside that element
                            break;
                        else
                            TreeViewItem = (TreeViewItem)TreeViewItem.Parent;
                        break;
                    // if new element, add name and traverse tree
                    case XmlNodeType.Element:
                        // determine if element contains content
                        if (!reader.IsEmptyElement)
                        {
                            // assign node text, add newNode as child
                            if (reader.HasAttributes)
                            {//QuizId
                                if (reader.GetAttribute("id") != null)
                                     newNode.Header = reader.Name + " no. " + reader.GetAttribute("id");
                                    //newNode.Header = reader.GetAttribute("id");
                               
                                else if (reader.GetAttribute("Correct") != null)
                                    newNode.Header = reader.Name + " [Correct Answer]";
                                else if (reader.GetAttribute("QuizId") != null)
                                    newNode.Header = reader.Name + " id: " + reader.GetAttribute("QuizId");
                                else
                                    newNode.Header = reader.Name;
                            }
                            else
                            {
                                if ((reader.Name == "Questi"))
                                    break;
                                else
                                    newNode.Header = reader.Name;
                            }
                            TreeViewItem.Items.Add(newNode);
                            // set TreeViewItem to last child
                            TreeViewItem = newNode;
                        }
                        else // do not traverse empty elements
                        {

                            newNode.Header = reader.NodeType.ToString();
                            TreeViewItem.Items.Add(newNode);
                        } // end else
                        break;
                    default: // all other types, display node type
                        //newNode.Header = reader.NodeType.ToString();
                        // TreeViewItem.Items.Add(newNode);
                        break;
                }
                newNode = new TreeViewItem();
            }
        }
    }
}
