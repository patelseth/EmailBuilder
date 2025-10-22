import React, { useState } from 'react';
import UnlayerEmailEditor from './UnlayerEmailEditor';
import axios from 'axios';

// Extend window type for Unlayer
declare global {
  interface Window {
    unlayer: any;
  }
}

// API host and endpoint for sending emails
const API_HOST = 'http://localhost:5034';
const API_URL = '/api/email/send';

/**
 * EmailBuilder component
 * - Embeds Unlayer email editor
 * - Captures recipient, subject, and HTML content
 * - Sends email via backend API using axios
 */
const EmailBuilder: React.FC = () => {
  // State for recipient, HTML content, subject, and status message
  const [recipient, setRecipient] = useState('');
  const [htmlContent, setHtmlContent] = useState('');
  const [subject, setSubject] = useState('');
  const [status, setStatus] = useState<string | null>(null);

  /**
   * Sends the email using the backend API
   */
  const handleSend = async () => {
    console.log('handleSend called!');
    console.log('Current state:', { recipient, subject, htmlContent });
    
    setStatus('Sending email...');
    
    // Auto-export HTML from Unlayer if not already exported
    if (!htmlContent && window.unlayer) {
      console.log('Attempting to export HTML from Unlayer...');
      try {
        // Check if Unlayer is properly initialized
        if (typeof window.unlayer.exportHtml !== 'function') {
          throw new Error('Unlayer exportHtml function not available');
        }
        
        // Check if editor container has content (indicator it's loaded)
        const editorContainer = document.getElementById('unlayer-editor-root');
        if (!editorContainer || editorContainer.children.length === 0) {
          throw new Error('Unlayer editor not fully loaded');
        }
        
        const exportedData = await new Promise<any>((resolve, reject) => {
          // Add a timeout to prevent hanging
          const timeout = setTimeout(() => {
            reject(new Error('Unlayer export timeout - please click "Use This Design" first'));
          }, 3000);
          
          window.unlayer.exportHtml((data: any) => {
            clearTimeout(timeout);
            console.log('Exported data:', data);
            if (data && data.html) {
              resolve(data);
            } else {
              reject(new Error('Invalid export data received'));
            }
          });
        });
        setHtmlContent(exportedData.html);
        
        console.log('Sending email with exported content...');
        // Send email with the newly exported content
        await axios.post(`${API_HOST}${API_URL}`, {
          recipient,
          htmlContent: exportedData.html,
          subject,
        });
        console.log('Email sent successfully with exported content!');
      } catch (err: any) {
        console.error('Error in export/send path:', err);
        setStatus('Failed to export email content: ' + err.message + '. Please click "Use This Design" first.');
        return;
      }
    } else {
      // Send with existing content
      console.log('Sending email with existing content...');
      try {
        await axios.post(`${API_HOST}${API_URL}`, {
          recipient,
          htmlContent,
          subject,
        });
        console.log('Email sent successfully with existing content!');
      } catch (err: any) {
        console.error('Error in existing content path:', err);
        setStatus('Failed to send email: ' + (err.response?.data || err.message));
        return;
      }
    }
    
    setStatus('Email sent successfully!');
  };

  return (
    <div className="email-builder-container">
      <h2>Email Builder</h2>
      <div>
        <label htmlFor="recipient">Recipient Email:</label>
        <input
          id="recipient"
          type="email"
          placeholder="Enter recipient email"
          value={recipient}
          onChange={e => setRecipient(e.target.value)}
        />
      </div>
      <div>
        <label htmlFor="subject">Subject:</label>
        <input
          id="subject"
          type="text"
          placeholder="Enter subject"
          value={subject}
          onChange={e => setSubject(e.target.value)}
        />
      </div>
      {/* Unlayer email editor for designing email content */}
      <div style={{ marginBottom: 16 }}>
        <UnlayerEmailEditor onHtmlExport={setHtmlContent} />
      </div>
      {/* Send button and status message below the editor */}
      <div style={{ marginTop: 24 }}>
        <button onClick={handleSend} disabled={!recipient || !subject}>
          Send Email
        </button>
        {(!recipient || !subject) && (
          <div style={{ color: '#666', fontSize: '14px', marginTop: '8px' }}>
            {!recipient && !subject ? 'Please enter recipient email and subject' :
             !recipient ? 'Please enter recipient email' :
             'Please enter subject'}
          </div>
        )}
        {/* Status message for success or error */}
        {status && <div className="status-message">{status}</div>}
      </div>
    </div>
  );
};

export default EmailBuilder;
