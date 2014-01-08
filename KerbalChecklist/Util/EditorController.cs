using System;
using System.Collections.Generic;
using UnityEngine;

namespace KerbalChecklist {

    [KSPAddon( KSPAddon.Startup.EditorAny, false )]
    public class EditorController : MonoBehaviour {

        private static bool active = false;
        private static EditorController _fetch;
        public static EditorController fetch {
            get {
                if( !active ) {
                    throw new ApplicationException( "EditorController has not been activated" );
                }

                return _fetch;
            }
        }

        public Part rootPart;
        public List<Part> attachedParts;
        public List<Part> sortedShipList;

        public static void Activate() {
            active = true;

            if( fetch != null ) {
                fetch.FixedUpdate();
            }
        }

        void Awake() {
            if( _fetch != null ) {
                throw new ApplicationException( "cannot run several instances simultaneously" );
            }

            _fetch = this;
        }

        void FixedUpdate() {
            if( !active ) {
                return;
            }

            UpdateSortedShipList();
            UpdateAttachedParts();
            UpdateRootPart();
        }

        void Destroy() {
            _fetch = null;
        }

        public void UpdateSortedShipList() {
            sortedShipList = new List<Part>();
            try {
                sortedShipList = EditorLogic.SortedShipList;
            } catch( NullReferenceException ignored ) {
                /* http://bugs.kerbalspaceprogram.com/issues/2021 */
            }
        }

        public void UpdateAttachedParts() {
            attachedParts = new List<Part>();

            // TODO check if this is going to be called several times
            foreach( Part part in sortedShipList ) {
                if( part.isAttached ) {
                    attachedParts.Add( part );
                }
            }
        }

        public void UpdateRootPart() {
            List<Part> parts = attachedParts;
            if( parts.Count == 0 || parts[0] == null ) {
                rootPart = null;
            } else {
                if( rootPart != parts[0] ) {
                    rootPart = parts[0];
                }
            }
        }

    }

}
