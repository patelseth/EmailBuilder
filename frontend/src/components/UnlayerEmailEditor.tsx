
// UnlayerEmailEditor component
// Embeds Unlayer email editor via CDN and provides HTML export to parent
import React, { useRef, useEffect } from 'react';

// Unlayer is loaded via CDN for simplicity
const UNLAYER_CDN = 'https://editor.unlayer.com/embed.js';


// Extend window type for Unlayer
declare global {
  interface Window {
    unlayer: any;
  }
}


// Props: callback to export HTML
interface UnlayerEmailEditorProps {
  onHtmlExport: (html: string) => void;
}


/**
 * UnlayerEmailEditor component
 * - Loads Unlayer editor via CDN
 * - Calls onHtmlExport with exported HTML when user clicks button
 */

const UnlayerEmailEditor: React.FC<UnlayerEmailEditorProps> = ({ onHtmlExport }) => {
  // Generate a unique id for this editor instance
  const uniqueIdRef = useRef(`unlayer-editor-${Math.random().toString(36).substr(2, 9)}`);
  const editorRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    // Load Unlayer script if not already loaded
    const initEditor = () => {
      window.unlayer.init({
        id: uniqueIdRef.current,
        displayMode: 'email',
      });
    };
    if (!window.unlayer) {
      const script = document.createElement('script');
      script.src = UNLAYER_CDN;
      script.async = true;
      script.onload = initEditor;
      document.body.appendChild(script);
    } else {
      initEditor();
    }
  }, []);


  // Export HTML from Unlayer and pass to parent
  const handleExport = () => {
    if (window.unlayer) {
      window.unlayer.exportHtml((data: any) => {
        onHtmlExport(data.html);
      });
    }
  };

  return (
    <div>
      {/* Unlayer editor container with unique id */}
      <div id={uniqueIdRef.current} ref={editorRef} style={{ height: 400, marginBottom: 16 }} />
      {/* Export button triggers HTML export */}
      <button type="button" onClick={handleExport} style={{ marginTop: 8 }}>
        Use This Design
      </button>
    </div>
  );
};

export default UnlayerEmailEditor;
