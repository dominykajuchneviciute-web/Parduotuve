import { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";

export default function OnlineUsers() {
  const [count, setCount] = useState<number | null>(null);

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5145/hubs/presence")
      .withAutomaticReconnect()
      .build();

    connection.on("OnlineCountChanged", (newCount: number) => {
      setCount(newCount);
    });

    connection.start().catch((err) => console.error("SignalR error:", err));

    return () => {
      connection.stop();
    };
  }, []);

  if (count === null) return null;

return (
  <span className="badge bg-success fw-normal fs-6 align-self-center px-3 py-2">
    Prisijungę: {count}
  </span>
);
}