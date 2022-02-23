mergeInto(LibraryManager.library, {
  SceneLoaded: function () {
    dispatchReactUnityEvent("SceneLoaded");
  },
  SaveMetadata: function(metadata) {
	dispatchReactUnityEvent("SaveMetadata", Pointer_stringify(metadata));
  },
});