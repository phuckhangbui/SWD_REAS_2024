import { createContext, useEffect, useState, FC } from "react";

// Define types for Cloudinary configuration and the setPublicId function
type CloudinaryConfig = {
  cloudName: string;
  uploadPreset: string;
  folder: string;
};

type SetPublicIdFunction = (publicId: string) => void;

// Define the context type
type CloudinaryScriptContextType = {
  loaded: boolean;
};

// Create a context to manage the script loading state
const CloudinaryScriptContext = createContext<CloudinaryScriptContextType>({
  loaded: false,
});

interface CloudinaryUploadWidgetProps {
  uwConfig: CloudinaryConfig;
  setPublicId: SetPublicIdFunction;
  setUploadedUrl: (url: string) => void; // Thêm prop để truyền đường dẫn ảnh đã upload đi
}

const CloudinaryUploadWidget: FC<CloudinaryUploadWidgetProps> = ({
  uwConfig,
  setPublicId,
  setUploadedUrl,
}) => {
  const [loaded, setLoaded] = useState(false);
  const [initialized, setInitialized] = useState(false);
  const [upUrl, setUrl] = useState<string>("");
  let myWidget: any;

  useEffect(() => {
    const initializeCloudinaryWidget = () => {
      if (!initialized) { //Dùng để check là đã init chưa, chưa mới tạo mới
        myWidget = (window as any).cloudinary.createUploadWidget(
          uwConfig,
          (error: any, result: any) => {
            if (!error && result && result.event === "success") {
              console.log("Done! Here is the image info: ", result.info);
              setPublicId(result.info.public_id);
              setUrl(result.info.secure_url);
              setUploadedUrl(result.info.secure_url);
            }
          }
        );

        document.getElementById("upload_widget")?.addEventListener(
          "click",
          () => {
            myWidget.open();
          },
          false
        );

        setInitialized(true);
      }
    };

    const scriptId = "uw";

    const loadScript = () => {
      const uwScript = document.getElementById(scriptId);
      if (!uwScript) {
        const script = document.createElement("script");
        script.setAttribute("async", "");
        script.setAttribute("id", scriptId);
        script.src = "https://upload-widget.cloudinary.com/global/all.js";
        script.addEventListener("load", () => {
          setLoaded(true);
          initializeCloudinaryWidget();
        });
        script.addEventListener("error", () => {
          console.error("Failed to load Cloudinary script.");
        });
        document.body.appendChild(script);
      } else {
        setLoaded(true);
        initializeCloudinaryWidget();
      }
    };

    loadScript();

    return () => {
      // Clean up event listener
      document
        .getElementById("upload_widget")
        ?.removeEventListener("click", () => {
          myWidget.open();
        });
    };
  }, [uwConfig, setPublicId, initialized, setUploadedUrl]);

  return (
    <CloudinaryScriptContext.Provider value={{ loaded }}>
      <button
        id="upload_widget"
        className="cloudinary-button"
      >
        Upload
      </button>
    </CloudinaryScriptContext.Provider>
  );
};

export default CloudinaryUploadWidget;
export type { CloudinaryConfig, SetPublicIdFunction };
export { CloudinaryScriptContext };

