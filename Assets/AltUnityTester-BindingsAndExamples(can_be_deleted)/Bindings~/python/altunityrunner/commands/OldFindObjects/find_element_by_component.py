from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
class FindElementByComponent(CommandReturningAltElements):
    def __init__(self, socket,request_separator,request_end,component_name,assembly_name,camera_name,enabled):
        super().__init__(socket,request_separator,request_end)
        self.component_name=component_name
        self.assembly_name=assembly_name
        self.camera_name=camera_name
        self.enabled=enabled
    
    def execute(self):
        if self.enabled==True:
            data = self.send_data(self.create_command('findObjectByComponent',self.assembly_name, self.component_name , self.camera_name ,'true'))
        else:
            data = self.send_data(self.create_command('findObjectByComponent',self.assembly_name, self.component_name , self.camera_name ,'false'))
        return self.get_alt_element(data)