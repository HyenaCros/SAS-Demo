import React from "react";
import ReactDOM from "react-dom/client";
import {
  BrowserRouter,
  Navigate,
  Route,
  RouterProvider,
  Routes,
} from "react-router-dom";
import ErrorLog from "./components/ErrorLog";
import FileUploadDetail from "./components/FileUploadDetail";
import FileUploadView from "./components/FileUploadView";
import Root from "./components/Root";

ReactDOM.createRoot(document.getElementById("root")).render(
  <React.StrictMode>
    <BrowserRouter>
      <Routes>
        <Route path="/*" element={<Root/>}>
          <Route path="errors" element={<ErrorLog/>}/>
          <Route index path="upload" element={<FileUploadView/>}/>
          <Route path="detail/:id" element={<FileUploadDetail/>}/>
          <Route path="*" element={<Navigate to="/upload"/>}/>
        </Route>
      </Routes>
    </BrowserRouter>
  </React.StrictMode>
);