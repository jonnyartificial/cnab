import React, { useEffect, useState } from "react";
import axios from "axios";

type Store = {
  id: number;
  name: string;
};

interface StoreDropdownProps {
  onSelect: (storeId: number) => void;
  refreshKey?: number;
}

const apiUrl = import.meta.env.VITE_APP_API_URL;

const StoreDropdown: React.FC<StoreDropdownProps> = ({
  onSelect,
  refreshKey,
}) => {
  const [stores, setStores] = useState<Store[]>([]);

  useEffect(() => {
    axios
      .get<Store[]>(`${apiUrl}/api/store`)
      .then((res) => setStores(res.data))
      .catch((err) => console.error("Error loading stores", err));
  }, [refreshKey]);

  return (
    <div>
      <label>Store: </label>
      <select
        onChange={(e) => onSelect(Number(e.target.value))}
        defaultValue=""
        style={{ minWidth: "300px" }}
      >
        <option value="">-- SELECT A STORE --</option>
        {stores.map((store) => (
          <option key={store.id} value={store.id}>
            {store.name}
          </option>
        ))}
      </select>
    </div>
  );
};

export default StoreDropdown;
