import React, { useRef, useState } from "react";
import axios from "axios";

interface FileUploadResult {
  isSuccess: boolean;
  linesRead: number;
  errors: number;
  imported: number;
  messages: string[];
}

interface FileUploaderProps {
  onUploadSuccess: () => void;
  style?: React.CSSProperties;
}

const apiUrl = import.meta.env.VITE_APP_API_URL;

const FileUploader: React.FC<FileUploaderProps> = ({
  onUploadSuccess,
  style,
}) => {
  const [uploading, setUploading] = useState(false);
  const [result, setResult] = useState<FileUploadResult | null>(null);
  const [uploadError, setUploadError] = useState(false);
  const dialogRef = useRef<HTMLDialogElement>(null);

  const handleFileUpload = async (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const file = event.target.files?.[0];
    if (!file) return;

    const formData = new FormData();
    formData.append("file", file);

    try {
      setUploading(true);
      setUploadError(false);
      setResult(null);

      const res = await axios.post<FileUploadResult>(
        `${apiUrl}/api/fileupload/cnab`,
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );

      setResult(res.data);
      dialogRef.current?.showModal();
      onUploadSuccess();
    } catch (error) {
      console.error("Upload failed", error);
      setUploadError(true);
      dialogRef.current?.showModal();
    } finally {
      setUploading(false);
      event.target.value = "";
    }
  };

  const closeDialog = () => {
    dialogRef.current?.close();
    setResult(null);
    setUploadError(false);
  };

  return (
    <div style={{ marginTop: "1rem", ...style }}>
      <label htmlFor="fileUpload">Update CNAB File: </label>
      <input
        id="fileUpload"
        type="file"
        accept=".txt"
        onChange={handleFileUpload}
        disabled={uploading}
      />

      <dialog ref={dialogRef} style={{ width: "600px" }}>
        <div>
          {uploadError ? (
            <p style={{ textAlign: "center", color: "red" }}>
              Upload failed. Please try again.
            </p>
          ) : result ? (
            <div style={{ textAlign: "left" }}>
              <strong>CNAB File Import Summary</strong>
              <div>
                <div>Lines Read: {result.linesRead}</div>
                <div>Errors: {result.errors}</div>
                <div>Imported: {result.imported}</div>
              </div>
              <div>Error Messages:</div>
              {result.messages.length > 0 && (
                <div
                  style={{
                    border: "1px solid #ccc",
                    marginTop: "10px",
                    height: "150px",
                    overflowY: "auto",
                    overflowX: "hidden",
                  }}
                >
                  {result.messages.map((msg, idx) => (
                    <li key={idx}>{msg}</li>
                  ))}
                </div>
              )}
            </div>
          ) : null}
        </div>
        <div style={{ marginTop: "1rem" }}>
          <button onClick={closeDialog}>Close</button>
        </div>
      </dialog>
    </div>
  );
};

export default FileUploader;
