import { BrowserRouter, Routes, Route } from "react-router-dom";
import { AuthProvider } from "./AuthContext";
import Layout from "./components/Layout";
import Home from "./pages/Home";
import Items from "./pages/Items";
import ItemDetails from "./pages/ItemDetails";
import MyItems from "./pages/MyItems";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Chats from "./pages/Chats";
import ChatDetails from "./pages/ChatDetails";

export default function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Layout />}>
            <Route index element={<Home />} />
            <Route path="items" element={<Items />} />
            <Route path="item/:id" element={<ItemDetails />} />
            <Route path="my-items" element={<MyItems />} />
            <Route path="login" element={<Login />} />
            < Route path = "register" element = {< Register />} />
            < Route path = "chats" element = {< Chats />} />
            < Route path = "chats/:id" element = {< ChatDetails />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}