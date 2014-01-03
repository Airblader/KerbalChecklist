using System;
using KerbalChecklist;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace KerbalChecklistUnitTest {

    [TestClass]
    public class ChecklistUnitTest {

        [TestMethod]
        public void TestGetItemsRecursively() {
            Checklist innerInnerList = new Checklist( "Inner Inner List" );
            innerInnerList.AddItem( new Item( "Inner Inner List Item", "" ) );

            Checklist innerList = new Checklist( "Inner List" );
            innerList.AddItem( new Item( "Inner List Item", "" ) );
            innerList.AddChecklist( innerInnerList );

            Checklist outerList = new Checklist( "Outer List" );
            outerList.AddItem( new Item( "Outer List Item", "" ) );
            outerList.AddChecklist( innerList );

            Checklists checklists = new Checklists();
            checklists.AddChecklist( innerInnerList );
            checklists.AddChecklist( innerList );
            checklists.AddChecklist( outerList );

            List<Item> allItems = outerList.GetItemsRecursively( checklists );
            Assert.AreEqual( 3, allItems.Count );
            Assert.AreEqual( "Outer List Item", allItems[0].name );
            Assert.AreEqual( "Inner List Item", allItems[1].name );
            Assert.AreEqual( "Inner Inner List Item", allItems[2].name );
        }

    }

}
