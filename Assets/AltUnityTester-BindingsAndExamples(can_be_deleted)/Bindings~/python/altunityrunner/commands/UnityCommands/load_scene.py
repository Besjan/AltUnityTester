from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
class LoadScene(CommandReturningAltElements):
    def __init__(self, socket,request_separator,request_end,scene_name):
        super().__init__(socket,request_separator,request_end)
        self.scene_name=scene_name
    
    def execute(self):
        data = self.send_data(self.create_command('loadScene', self.scene_name))
        if (data == 'Ok'):
            print('Scene loaded: ' + self.scene_name)
            return data
        return None