import React from "react";
import { createBrowserRouter, RouteObject } from "react-router-dom";
import ErrorLog from "./components/ErrorLog";
import FileUploadDetail from "./components/FileUploadDetail";
import FileUploadView from "./components/FileUploadView";
import FileWatcher from "./components/FileWatcher";
import Root from "./components/Root";

const rootPath: RouteObject = {
  path: "/",
  element: <Root />,
};

const routes: RouteObject[] = [
  {
    path: "errors",
    element: <ErrorLog />
  },
  {
    path: "upload",
    element: <FileUploadView />
  },
  {
    path: "detail/:id",
    element: <FileUploadDetail/>
  }
]

rootPath.children = routes;

export const router = createBrowserRouter([rootPath]);