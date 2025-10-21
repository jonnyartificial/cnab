import { useState } from "react";
import "./App.css";
import StoreDropdown from "./components/StoreDropdown";

function App() {
  const handleStoreSelect = (storeId: number) => {};

  return (
    <>
      <StoreDropdown onSelect={handleStoreSelect} />
    </>
  );
}

export default App;
