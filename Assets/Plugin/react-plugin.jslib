mergeInto(LibraryManager.library, {
  SceneLoaded: function () {
    dispatchReactUnityEvent("SceneLoaded");
  },
  SaveMetadata: function(metadata) {
	dispatchReactUnityEvent("SaveMetadata", Pointer_stringify(metadata));
  },
  SaveScreenshot: function(byteArray, size) {
	const newArray = new ArrayBuffer(size);
	const newByteArray = new Uint8Array(newArray);
	for (var i = 0; i < size; i++) {
		newByteArray[i] = HEAPU8[byteArray + i];
	}
	dispatchReactUnityEvent("SaveScreenshot", newByteArray);
  }
});