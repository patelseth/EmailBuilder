import React, { useState } from 'react';
import UnlayerEmailEditor from './UnlayerEmailEditor';
import axios from 'axios';


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
    setStatus(null);
    try {
      await axios.post(`${API_HOST}${API_URL}`, {
        recipient,
        htmlContent,
        subject,
      });
      setStatus('Email sent successfully!');
    } catch (err: any) {
      setStatus('Failed to send email: ' + (err.response?.data || err.message));
    }
  };

  return (
    <div style={{ maxWidth: 700, margin: '2rem auto', padding: 24, border: '1px solid #eee', borderRadius: 8 }}>
      <h2>Email Builder</h2>
      <div style={{ marginBottom: 16 }}>
        <label htmlFor="recipient">Recipient Email:</label>
        <input
          id="recipient"
          type="email"
          placeholder="Enter recipient email"
          value={recipient}
          onChange={e => setRecipient(e.target.value)}
          style={{ width: '100%', padding: 8, marginTop: 4 }}
        />
      </div>
      <div style={{ marginBottom: 16 }}>
        <label htmlFor="subject">Subject:</label>
        <input
          id="subject"
          type="text"
          placeholder="Enter subject"
          value={subject}
          onChange={e => setSubject(e.target.value)}
          style={{ width: '100%', padding: 8, marginTop: 4 }}
        />
      </div>
      {/* Unlayer email editor for designing email content */}
      <div style={{ marginBottom: 16 }}>
        <UnlayerEmailEditor onHtmlExport={setHtmlContent} />
      </div>
      {/* Send button and status message below the editor */}
      <div style={{ marginTop: 24 }}>
        <button style={{ padding: '10px 24px', fontSize: 16 }} onClick={handleSend} disabled={!recipient || !htmlContent || !subject}>
          Send Email
        </button>
        {/* Status message for success or error */}
        {status && <div style={{ marginTop: 16 }}>{status}</div>}
      </div>
    </div>
  );
};

export default EmailBuilder;
