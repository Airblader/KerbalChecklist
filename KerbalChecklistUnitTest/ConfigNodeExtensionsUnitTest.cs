using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KerbalChecklist.Extensions;

namespace KerbalChecklistUnitTest {

    [TestClass]
    public class ConfigNodeExtensionsUnitTest {

        [TestMethod]
        public void TestGetBooleanValue() {
            AssertGetBooleanValue( "true", true );
            AssertGetBooleanValue( "True", true );
            AssertGetBooleanValue( "TRUE", true );

            AssertGetBooleanValue( "false", false );
            AssertGetBooleanValue( "False", false );
            AssertGetBooleanValue( "FALSE", false );
        }

        private void AssertGetBooleanValue( string value, bool expected ) {
            ConfigNode node = new ConfigNode();
            node.AddValue( "test", value );

            Assert.AreEqual( expected, node.GetBooleanValue( "test" ) );
        }

        [TestMethod]
        public void TestRemoveNodesWithValue() {
            ConfigNode node = new ConfigNode();
            node.AddNode( CreateNodeWithNameAndProperty( "SUBNODE", "test", "foo" ) ); // should be removed
            node.AddNode( CreateNodeWithNameAndProperty( "SUBNODE", "baz", "foo" ) );
            node.AddNode( CreateNodeWithNameAndProperty( "SUBNODE", "test", "foo" ) ); // should be removed
            node.AddNode( CreateNodeWithNameAndProperty( "SUBNODE", "test", "bar" ) );
            node.AddNode( CreateNodeWithNameAndProperty( "NOPE", "test", "foo" ) );

            Assert.AreEqual( 5, node.nodes.Count ); // sanity check

            node.RemoveNodesWithValue( "SUBNODE", "test", "foo" );
            Assert.AreEqual( 3, node.nodes.Count );
        }

        [TestMethod]
        public void TestHasNodeWithValue() {
            Assert.IsTrue( WrapIntoNode( CreateNodeWithNameAndProperty( "SUBNODE", "test", "foo" ) )
                .HasNodeWithValue( "SUBNODE", "test", "foo" ) );

            Assert.IsFalse( WrapIntoNode( CreateNodeWithNameAndProperty( "BAZ", "test", "foo" ) )
                .HasNodeWithValue( "SUBNODE", "test", "foo" ) );

            Assert.IsFalse( WrapIntoNode( CreateNodeWithNameAndProperty( "SUBNODE", "baz", "foo" ) )
                .HasNodeWithValue( "SUBNODE", "test", "foo" ) );

            Assert.IsFalse( WrapIntoNode( CreateNodeWithNameAndProperty( "SUBNODE", "test", "baz" ) )
                .HasNodeWithValue( "SUBNODE", "test", "foo" ) );
        }

        private ConfigNode CreateNodeWithNameAndProperty( string name, string property, string value ) {
            ConfigNode node = new ConfigNode( name );
            node.AddValue( property, value );

            return node;
        }

        private ConfigNode WrapIntoNode( ConfigNode node ) {
            ConfigNode config = new ConfigNode();
            config.AddNode( node );

            return config;
        }

    }

}
