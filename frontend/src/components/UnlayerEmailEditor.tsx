
// UnlayerEmailEditor component
// Embeds Unlayer email editor via CDN and provides HTML export to parent
import React, { useRef, useEffect, useState } from 'react';

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
  // Use a static id for the editor container
  const EDITOR_ID = 'unlayer-editor-root';
  const editorRef = useRef<HTMLDivElement>(null);
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    // Only initialize if not already present
    const initEditor = () => {
      if (!window.unlayer || document.getElementById(EDITOR_ID)?.children.length) return;
      
      window.unlayer.init({
        id: EDITOR_ID,
        displayMode: 'email',
      });
      
      // Set ready state after a short delay to ensure editor is fully loaded
      setTimeout(() => {
        setIsReady(true);
        console.log('Unlayer editor is ready');
      }, 2000);
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
    // No destroy on unmount to avoid breaking global singleton
  }, []);


  // Export HTML from Unlayer and pass to parent
  const handleExport = () => {
    if (window.unlayer && isReady) {
      console.log('Exporting HTML from Unlayer...');
      window.unlayer.exportHtml((data: any) => {
        console.log('Export successful:', data);
        onHtmlExport(data.html);
      });
    } else {
      console.warn('Unlayer not ready yet');
    }
  };

  return (
    <div>
      {/* Unlayer editor container with static id */}
      <div id={EDITOR_ID} ref={editorRef} style={{ height: 400, marginBottom: 16 }} />
      {/* Export button triggers HTML export */}
      <button 
        type="button" 
        onClick={handleExport} 
        style={{ 
          marginTop: 8,
          backgroundColor: isReady ? '#007bff' : '#ccc',
          color: 'white',
          border: 'none',
          padding: '8px 16px',
          borderRadius: '4px',
          cursor: isReady ? 'pointer' : 'not-allowed'
        }}
        disabled={!isReady}
      >
        {isReady ? 'Use This Design' : 'Loading Editor...'}
      </button>
      {isReady && (
        <div style={{ color: '#666', fontSize: '12px', marginTop: '4px' }}>
          ✅ Editor ready - you can now export your design
        </div>
      )}
    </div>
  );
};

export default UnlayerEmailEditor;
