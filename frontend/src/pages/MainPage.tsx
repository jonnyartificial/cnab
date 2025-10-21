import React, { useState } from "react";
import StoreDropdown from "../components/StoreDropdown";
import type { AccountTransaction } from "../types/AccountTransaction";
import axios from "axios";
import TransactionGrid from "../components/TransactionGrid";
import FileUploader from "../components/FileUploader";

const apiUrl = import.meta.env.VITE_APP_API_URL;

const MainPage: React.FC = () => {
  const [selectedStoreId, setSelectedStoreId] = useState<number | null>(null);
  const [transactions, setTransactions] = useState<AccountTransaction[]>([]);
  const [storeRefreshKey, setStoreRefreshKey] = useState<number>(0);

  const loadTransactions = async (storeId: number) => {
    try {
      const res = await axios.get<AccountTransaction[]>(
        `${apiUrl}/api/accounttransaction?storeId=${storeId}`
      );
      setTransactions(res.data);
    } catch (error) {
      console.error("Failed to load transactions", error);
      setTransactions([]);
    }
  };

  const handleStoreSelect = (storeId: number) => {
    setSelectedStoreId(storeId);
    loadTransactions(storeId);
  };

  const handleUploadSuccess = () => {
    if (selectedStoreId !== null) {
      loadTransactions(selectedStoreId);
    }
    setStoreRefreshKey((p) => p + 1);
  };

  return (
    <div>
      <h2>CNAB Transactions by Store</h2>
      <FileUploader
        onUploadSuccess={handleUploadSuccess}
        style={{ textAlign: "right" }}
      />
      <StoreDropdown
        onSelect={handleStoreSelect}
        refreshKey={storeRefreshKey}
      />
      <TransactionGrid transactions={transactions} />
    </div>
  );
};

export default MainPage;
