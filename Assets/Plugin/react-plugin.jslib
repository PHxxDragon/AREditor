mergeInto(LibraryManager.library, {
  SceneLoaded: function () {
    dispatchReactUnityEvent("SceneLoaded");
  },
});