import os

import pytest

from .utils import Scenes
from altunityrunner import By
from altunityrunner.__version__ import VERSION
from altunityrunner.commands import GetServerVersion
from altunityrunner.logging import AltUnityLogLevel, AltUnityLogger
import altunityrunner.exceptions as exceptions


class TestDriver:

    @pytest.fixture(autouse=True)
    def setup(self, altdriver):
        self.altdriver = altdriver

    def test_get_version(self):
        server_version = GetServerVersion.run(self.altdriver._connection)
        assert VERSION.startswith(server_version)

    def test_load_and_wait_for_scene(self):
        self.altdriver.load_scene(Scenes.Scene01)
        self.altdriver.wait_for_current_scene_to_be(Scenes.Scene01, timeout=1)

        self.altdriver.load_scene(Scenes.Scene02)
        self.altdriver.wait_for_current_scene_to_be(Scenes.Scene02, timeout=1)

        assert self.altdriver.get_current_scene() == Scenes.Scene02

    def test_wait_for_current_scene_to_be_with_a_non_existing_scene(self):
        scene_name = "AltUnityDriverTestScene"

        with pytest.raises(exceptions.WaitTimeOutException) as execinfo:
            self.altdriver.wait_for_current_scene_to_be(scene_name, timeout=1, interval=0.5)

        assert str(execinfo.value) == "Scene {} not loaded after 1 seconds".format(scene_name)

    def test_set_and_get_time_scale(self):
        self.altdriver.set_time_scale(0.1)

        assert self.altdriver.get_time_scale() == 0.1

        self.altdriver.set_time_scale(1)

    def test_screenshot(self):
        png_path = "testPython.png"
        self.altdriver.get_png_screenshot(png_path)
        assert os.path.exists(png_path)

    def test_wait_for_object_which_contains_with_tag(self):
        alt_unity_object = self.altdriver.wait_for_object_which_contains(
            By.NAME, "Canva",
            By.TAG, "MainCamera"
        )
        assert alt_unity_object.name == "Canvas"

    def test_load_additive_scenes(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene", load_single=True)

        initial_number_of_elements = self.altdriver.get_all_elements()
        self.altdriver.load_scene("Scene 2 Draggable Panel", load_single=False)
        final_number_of_elements = self.altdriver.get_all_elements()

        assert len(final_number_of_elements) > len(initial_number_of_elements)

        scenes = self.altdriver.get_all_loaded_scenes()
        assert len(scenes) == 2

    def test_load_scene_with_invalid_scene_name(self):
        with pytest.raises(exceptions.SceneNotFoundException):
            self.altdriver.load_scene("Scene 0")

    def test_unload_scene(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene", load_single=True)
        self.altdriver.load_scene("Scene 2 Draggable Panel", load_single=False)

        assert len(self.altdriver.get_all_loaded_scenes()) == 2

        self.altdriver.unload_scene("Scene 2 Draggable Panel")
        assert len(self.altdriver.get_all_loaded_scenes()) == 1
        assert self.altdriver.get_all_loaded_scenes()[0] == "Scene 1 AltUnityDriverTestScene"

    def test_unload_only_scene(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene", load_single=True)

        with pytest.raises(exceptions.CouldNotPerformOperationException):
            self.altdriver.unload_scene("Scene 1 AltUnityDriverTestScene")

    def test_set_server_logging(self):
        rule = self.altdriver.call_static_method(
            "Altom.AltUnityTester.Logging.ServerLogManager",
            "Instance.Configuration.FindRuleByName",
            ["AltUnityServerFileRule"],
            assembly="Assembly-CSharp"
        )

        # Default logging level in AltUnity Tester is Debug level
        assert len(rule["Levels"]) == 5

        self.altdriver.set_server_logging(AltUnityLogger.File, AltUnityLogLevel.Off)
        rule = self.altdriver.call_static_method(
            "Altom.AltUnityTester.Logging.ServerLogManager",
            "Instance.Configuration.FindRuleByName",
            ["AltUnityServerFileRule"],
            assembly="Assembly-CSharp")

        assert len(rule["Levels"]) == 0

        # Reset logging level
        self.altdriver.set_server_logging(AltUnityLogger.File, AltUnityLogLevel.Debug)

    @pytest.mark.parametrize(
        "path", ["//[1]", "CapsuleInfo[@tag=UI]", "//CapsuleInfo[@tag=UI/Text", "//CapsuleInfo[0/Text"]
    )
    def test_invalid_paths(self, path):
        with pytest.raises(exceptions.AltUnityInvalidPathException):
            self.altdriver.find_object(By.PATH, path)
