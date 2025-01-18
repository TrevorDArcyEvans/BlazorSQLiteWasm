export function synchronizeFileWithIndexedDb(filename) {
    return new Promise((res, rej) => {
        const db = window.indexedDB.open('SqliteStorage', 1);
        db.onupgradeneeded = () => {
            db.result.createObjectStore('Files', { keypath: 'id' });
        };

        db.onsuccess = () => {
            const req = db.result.transaction('Files', 'readonly').objectStore('Files').get('file');
            req.onsuccess = () => {
                Blazor.runtime.Module.FS_createDataFile('/', filename, req.result, true, true, true);
                res();
            };
        };

        let lastModifiedTime = new Date();
        setInterval(() => {
            const path = `/${filename}`;
            if (Blazor.runtime.Module.FS.analyzePath(path).exists) {
                const mtime = Blazor.runtime.Module.FS.stat(path).mtime;
                if (mtime.valueOf() !== lastModifiedTime.valueOf()) {
                    lastModifiedTime = mtime;
                    const data = Blazor.runtime.Module.FS.readFile(path);
                    db.result.transaction('Files', 'readwrite').objectStore('Files').put(data, 'file');
                }
            }
        }, 1000);
    });
}

