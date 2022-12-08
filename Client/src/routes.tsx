import React from "react";
import { createBrowserRouter, RouteObject } from "react-router-dom";
import ErrorLog from "./components/ErrorLog";
import FileUpload from "./components/FileUpload";
import FileWatcher from "./components/FileWatcher";
import Root from "./components/Root";

const rootPath: RouteObject = {
    path: "/",
    element: <Root/>,
};

const routes: RouteObject[] = [
    {
        path: "errors",
        element: <ErrorLog/>
    },
    {
        path: "upload",
        element: <FileUpload/>
    },
    {
        path: "watcher",
        element: <FileWatcher/>
    }
]

rootPath.children = routes;

export const router = createBrowserRouter([rootPath]);