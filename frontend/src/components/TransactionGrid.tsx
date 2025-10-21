import React from "react";
import { format } from "date-fns";
import type { AccountTransaction } from "../types/AccountTransaction";

interface TransactionGridProps {
  transactions: AccountTransaction[];
}

const thStyle: React.CSSProperties = {
  border: "1px solid #ccc",
  padding: "8px",
  backgroundColor: "#f5f5f5",
  textAlign: "left",
};

const tdStyle: React.CSSProperties = {
  border: "1px solid #ccc",
  padding: "8px",
};

const TransactionGrid: React.FC<TransactionGridProps> = ({ transactions }) => {
  if (transactions.length === 0) return null;

  return (
    <table
      style={{ marginTop: "1rem", borderCollapse: "collapse", width: "100%" }}
    >
      <thead>
        <tr>
          <th style={thStyle}>Date</th>
          <th style={thStyle}>Type</th>
          <th style={thStyle}>CPF</th>
          <th style={thStyle}>Card</th>
          <th style={thStyle}>Store</th>
          <th style={thStyle}>Owner</th>
          <th style={thStyle}>Value</th>
          <th style={thStyle}>Balance</th>
        </tr>
      </thead>
      <tbody>
        {transactions.map((tx) => (
          <tr key={tx.id}>
            <td style={tdStyle}>
              {format(new Date(tx.date), "yyyy-MM-dd HH:mm:ss")}
            </td>
            <td style={tdStyle}>{tx.type}</td>
            <td style={tdStyle}>{tx.cpf}</td>
            <td style={tdStyle}>{tx.card}</td>
            <td style={tdStyle}>{tx.storeName}</td>
            <td style={tdStyle}>{tx.storeOwner}</td>
            <td
              style={{
                ...tdStyle,
                textAlign: "right",
                color: tx.value < 0 ? "red" : "inherit",
              }}
            >
              {formatCurrencyBRL(tx.value)}
            </td>
            <td
              style={{
                ...tdStyle,
                textAlign: "right",
                color: tx.balance < 0 ? "red" : "inherit",
              }}
            >
              {formatCurrencyBRL(tx.balance)}
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
};

export function formatCurrencyBRL(value: number): string {
  if (isNaN(value)) return "R$ 0,00";

  return new Intl.NumberFormat("pt-BR", {
    style: "currency",
    currency: "BRL",
    minimumFractionDigits: 2,
  }).format(value);
}

export default TransactionGrid;
