# install nodejs from latest
echo "This script will add the nodejs ppa to your list of sources and install the latest nodejs build, mongodb, mocha, and grunt-cli. Do you want to continue?"
select yn in "Yes" "No"; do
	case $yn in
		Yes ) break;;
		No ) exit;
	esac
done
sudo add-apt-repository ppa:chris-lea/node.js && sudo apt-get update && sudo apt-get install python-software-properties nodejs mongodb && sudo npm install -g grunt-cli && sudo npm install -g mocha